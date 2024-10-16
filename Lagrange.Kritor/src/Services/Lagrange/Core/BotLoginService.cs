using System;
using System.IO;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Lagrange.Core;
using Lagrange.Core.Common.Interface.Api;
using Lagrange.Core.Event.EventArg;
using Lagrange.Kritor.Utilities;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using MsLogLevel = Microsoft.Extensions.Logging.LogLevel;

namespace Lagrange.Kritor.Services.Lagrange.Core;

public class BotLoginService(ILogger<BotLoginService> logger, BotContext bot) : IHostedService {
    public async Task StartAsync(CancellationToken token) {
        bot.Invoker.OnBotOnlineEvent += HandleBotOnlineEvent;

        // EasyLogin success
        if (bot.UpdateKeystore().Session.TempPassword != null && await bot.LoginByPassword(token)) return;

        // QRCode Login
        (string url, _) = await bot.FetchQrCode() ?? throw new Exception("Fetch qr code failed");
        logger.LogQrCode(QrCodeUtility.BuildConsoleString(url));

        await bot.LoginByQrCode(token);
    }

    public Task StopAsync(CancellationToken cancellationToken) {
        bot.Invoker.OnBotOnlineEvent -= HandleBotOnlineEvent;

        return Task.CompletedTask;
    }

    private void HandleBotOnlineEvent(BotContext _, BotOnlineEvent e) {
        File.WriteAllText("device.json", JsonSerializer.Serialize(bot.UpdateDeviceInfo()));
        File.WriteAllText("keystore.json", JsonSerializer.Serialize(bot.UpdateKeystore()));

        logger.LogLoginSuccess(bot.BotUin);
    }
}

public static partial class LoginServiceLogger {
    [LoggerMessage(EventId = 100, Level = MsLogLevel.Information, Message = "\n{qrcode}")]
    public static partial void LogQrCode(this ILogger<BotLoginService> logger, string qrcode);

    [LoggerMessage(EventId = 200, Level = MsLogLevel.Information, Message = "Bot {uin} Logged")]
    public static partial void LogLoginSuccess(this ILogger<BotLoginService> logger, uint uin);
}