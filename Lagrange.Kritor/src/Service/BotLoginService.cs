using System;
using System.IO;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Lagrange.Core;
using Lagrange.Core.Common.Interface.Api;
using Lagrange.Kritor.Utility;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Lagrange.Kritor.Service;

public class BotLoginService(ILogger<BotLoginService> logger, IHostApplicationLifetime lifetime, BotContext bot) : IHostedService {
    private readonly IHostApplicationLifetime _lifetime = lifetime;

    public async Task StartAsync(CancellationToken token) {
        bot.Invoker.OnBotOnlineEvent += HandleBotOnlineEvent;

        // EasyLogin success
        if (bot.UpdateKeystore().Session.TempPassword is not null && bot.LoginByPassword().Result) return;

        // QRCode Login
        (string url, _) = bot.FetchQrCode().Result ?? throw new Exception("Failed to fetch QR code");
        logger.LogQrCode(QRCodeUtility.BuildConsoleString(url));

        await bot.LoginByQrCode().WaitAsync(token);
    }

    public Task StopAsync(CancellationToken cancellationToken) {
        bot.Invoker.OnBotOnlineEvent -= HandleBotOnlineEvent;

        return Task.CompletedTask;
    }

    private void HandleBotOnlineEvent(BotContext bot, Core.Event.EventArg.BotOnlineEvent e) {
        File.WriteAllText("device.json", JsonSerializer.Serialize(bot.UpdateDeviceInfo()));
        File.WriteAllText("keystore.json", JsonSerializer.Serialize(bot.UpdateKeystore()));

        logger.LogLoginSuccess(bot.BotUin);
    }
}

public static partial class LoginServiceLogger {
    [LoggerMessage(EventId = 100, Level = LogLevel.Information, Message = "{qrcode}")]
    public static partial void LogQrCode(this ILogger<BotLoginService> logger, string qrcode);

    [LoggerMessage(EventId = 200, Level = LogLevel.Information, Message = "Bot {uin} Logged")]
    public static partial void LogLoginSuccess(this ILogger<BotLoginService> logger, uint uin);
}