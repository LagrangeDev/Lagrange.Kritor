using System;
using System.IO;
using System.Net;
using System.Text;
using System.Text.Json;
using Lagrange.Core;
using Lagrange.Core.Common;
using Lagrange.Core.Common.Interface;
using Lagrange.Core.Utility.Sign;
using Lagrange.Kritor.Providers;
using Lagrange.Kritor.Services.Lagrange.Core;
using Lagrange.Kritor.Services.Kritor.Grpc.Authentication;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Linq;
using Lagrange.Kritor.Interceptors;
using Lagrange.Kritor.Services.Kritor.Grpc.Core;
using Lagrange.Kritor.Services.Kritor.Grpc.Event;
using System.Reflection;
using Lagrange.Kritor.Authenticates;
using Lagrange.Kritor.Services.Kritor.Grpc.File;
using Lagrange.Kritor.Services.Kritor.Grpc.Group;
using Lagrange.Kritor.Services.Kritor.Grpc.Guild;
using Lagrange.Kritor.Services.Kritor.Grpc.Message;

namespace Lagrange.Kritor;
internal class Program {
    private static void Main(string[] args) {
        // UTF8
        Console.OutputEncoding = Encoding.UTF8;

        // Print Version
        string version = Assembly.GetAssembly(typeof(Program))?
            .GetCustomAttribute<AssemblyInformationalVersionAttribute>()?
            .InformationalVersion ?? "Unknown";
        Console.WriteLine($"Lagrange.Kritor Version: {version}");

        WebApplicationBuilder builder = WebApplication.CreateBuilder(args);
        builder.WebHost.ConfigureKestrel((context, sOptions) => {
            IConfigurationSection kritorSection = context.Configuration
                .GetRequiredSection("Kritor")
                .GetRequiredSection("Network");

            string? addressString = kritorSection.GetRequiredSection("Address").Get<string>();
            if (!IPAddress.TryParse(addressString, out IPAddress? address)) {
                throw new Exception($"Unknown Address({addressString})");
            }

            int port = kritorSection.GetRequiredSection("Port").Get<int>();
            if (port < 1 || port > 65535) throw new Exception($"Port({port}) out of range(1 - 65565)");

            sOptions.Listen(address, port, (lOptions) => lOptions.Protocols = HttpProtocols.Http2);
        });
        builder.Services.AddGrpc((options) => {
            options.Interceptors.Add<AuthenticatorInterceptor>();
            options.Interceptors.Add<KritorVersionInterceptor>();
        });

        builder.Services.AddSingleton(SignProviderFactory);
        builder.Services.AddSingleton(BotConfigFactory);
        builder.Services.AddSingleton(BotDeviceInfoFactory);
        builder.Services.AddSingleton(BotKeystoreFactory);
        builder.Services.AddSingleton(BotContextFactory);

        builder.Services.AddSingleton(AuthenticatorFactory);

        builder.Services.AddHostedService<BotLoggerService>();
        builder.Services.AddHostedService<BotLoginService>();

        var app = builder.Build();

        app.MapGrpcService<KritorAuthenticationService>();
        app.MapGrpcService<KritorCoreService>();
        app.MapGrpcService<KritorEventService>();
        app.MapGrpcService<KritorGroupFileService>();
        app.MapGrpcService<KritorGroupService>();
        app.MapGrpcService<KritorGuildService>();
        app.MapGrpcService<KritorMessageService>();
        app.MapGet("/", () => "Hey kid, you're supposed to use gRPC to access it, not a browser.");

        app.Run();
    }

    private static SignProvider SignProviderFactory(IServiceProvider provider) {
        IConfiguration config = provider.GetRequiredService<IConfiguration>()
                .GetRequiredSection("Core")
                .GetRequiredSection("Protocol")
                .GetRequiredSection("Signer");

        return new KritorSignerProvider(config.GetSection("Url").Get<string>(), config.GetSection("Proxy").Get<string>());
    }

    private static BotConfig BotConfigFactory(IServiceProvider provider) {
        IConfiguration coreConfig = provider.GetRequiredService<IConfiguration>().GetRequiredSection("Core");
        IConfiguration protocolConfig = coreConfig.GetRequiredSection("Protocol");
        IConfiguration serverConfig = coreConfig.GetRequiredSection("Server");

        return new BotConfig() {
            Protocol = protocolConfig.GetSection("Platform").Get<string>() switch {
                "Windows" => Protocols.Windows,
                "MacOs" => Protocols.MacOs,
                "Linux" or null => Protocols.Linux,
                string protocol => throw new NotSupportedException($"Not supported Core.Protocol.Server.Platform({protocol})")
            },
            AutoReconnect = serverConfig.GetSection("AutoReconnect").Get<bool>(),
            GetOptimumServer = serverConfig.GetSection("GetOptimumServer").Get<bool>(),
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

    private static Authenticator AuthenticatorFactory(IServiceProvider provider) {
        ILogger<Authenticator> logger = provider.GetRequiredService<ILogger<Authenticator>>();

        IConfigurationSection config = provider.GetRequiredService<IConfiguration>()
            .GetRequiredSection("Kritor")
            .GetRequiredSection("Authentication");

        BotContext bot = provider.GetRequiredService<BotContext>();

        bool enabled = config.GetRequiredSection("Enabled").Get<bool>();

        return new(
            enabled,
            enabled
                ? config.GetRequiredSection("SuperTicket").Get<string>()
                    ?? throw new Exception(
                        "When Kritor.Authentication.Enabled is true, Kritor.Authentication.SuperTicket cannot be null"
                    )
                : "",
            bot.BotUin.ToString(),
            enabled
                ? config.GetRequiredSection("Tickets").GetChildren()
                    .Select((tickets) => {
                        return tickets.Get<string>()
                            ?? throw new Exception(
                                "When Kritor.Authentication.Enabled is true, Kritor.Authentication.Tickets cannot be null"
                            );
                    }).ToArray()
                : []
        );
    }
}