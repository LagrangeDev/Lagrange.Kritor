
using System;
using System.IO;
using System.Net;
using System.Text;
using System.Text.Json;
using Lagrange.Core;
using Lagrange.Core.Common;
using Lagrange.Core.Common.Interface;
using Lagrange.Core.Utility.Sign;
using Lagrange.Kritor.Provider;
using Lagrange.Kritor.Service;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Lagrange.Kritor;
internal class Program {
    private static void Main(string[] args) {
        // UTF8
        Console.OutputEncoding = Encoding.UTF8;


        WebApplicationBuilder builder = WebApplication.CreateBuilder(args);
        builder.WebHost.ConfigureKestrel((context, sOptions) => {
            IConfigurationSection kritorSection = context.Configuration.GetSection("Kritor");

            if (!IPAddress.TryParse(kritorSection.GetSection("Address").Get<string>(), out IPAddress? address)) {
                throw new Exception("Kritor.Address must not be null and can be resolved to IPAddress");
            }

            int port = kritorSection.GetSection("Port").Get<int>();
            if (port is < 1 or > 65535) throw new Exception("Kritor.Port must not be null and be in the range 1-65535");

            sOptions.Listen(address, port, (lOptions) => lOptions.Protocols = HttpProtocols.Http2);
        });

        builder.Services.AddSingleton(SignProviderFactory);
        builder.Services.AddSingleton(BotConfigFactory);
        builder.Services.AddSingleton(BotDeviceInfoFactory);
        builder.Services.AddSingleton(BotKeystoreFactory);
        builder.Services.AddSingleton(BotContextFactory);

        builder.Services.AddHostedService<BotLoggerService>();
        builder.Services.AddHostedService<BotLoginService>();

        builder.Services.AddGrpc();

        var app = builder.Build();

        app.MapGet("/", () => "Hey kid, you're supposed to use gRPC to access it, not a browser.");

        app.Run();
    }

    private static SignProvider SignProviderFactory(IServiceProvider provider) {
        IConfiguration config = provider.GetRequiredService<IConfiguration>()
                .GetRequiredSection("Core")
                .GetRequiredSection("Protocol");

        return new KritorSignerProvider(config.GetSection("SignerUrl").Get<string>());
    }

    private static BotConfig BotConfigFactory(IServiceProvider provider) {
        IConfiguration config = provider
            .GetRequiredService<IConfiguration>()
            .GetRequiredSection("Core")
            .GetRequiredSection("Protocol");

        return new BotConfig() {
            Protocol = config.GetSection("Platform").Get<string>() switch {
                "Windows" => Protocols.Windows,
                "MacOs" => Protocols.MacOs,
                "Linux" or null => Protocols.Linux,
                string protocol => throw new Exception($"Unknown Core.Protocol.Platform: {protocol}")
            },
            AutoReconnect = config.GetSection("AutoReconnect").Get<bool>(),
            GetOptimumServer = config.GetSection("GetOptimumServer").Get<bool>(),
            CustomSignProvider = provider.GetRequiredService<SignProvider>(),
        };
    }

    private static BotDeviceInfo BotDeviceInfoFactory(IServiceProvider _) {
        if (!File.Exists("device.json")) {
            BotDeviceInfo device = BotDeviceInfo.GenerateInfo();
            File.WriteAllText("device.json", JsonSerializer.Serialize(device));
            return device;
        }

        return JsonSerializer.Deserialize<BotDeviceInfo>(File.ReadAllText("device.json"))
            ?? throw new Exception("Unable to deserialize device.json");
    }

    private static BotKeystore BotKeystoreFactory(IServiceProvider _) {
        if (!File.Exists("keystore.json")) {
            BotKeystore keystore = new();
            File.WriteAllText("keystore.json", JsonSerializer.Serialize(keystore));
            return keystore;
        }

        return JsonSerializer.Deserialize<BotKeystore>(File.ReadAllText("keystore.json"))
            ?? throw new Exception("Unable to deserialize keystore.json");
    }

    private static BotContext BotContextFactory(IServiceProvider provider) {
        BotConfig config = provider.GetRequiredService<BotConfig>();
        BotDeviceInfo device = provider.GetRequiredService<BotDeviceInfo>();
        BotKeystore keystore = provider.GetRequiredService<BotKeystore>();

        return BotFactory.Create(config, device, keystore);
    }
}