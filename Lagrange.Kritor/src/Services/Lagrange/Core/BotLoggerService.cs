using System.Threading;
using System.Threading.Tasks;
using Lagrange.Core;
using CoreLogLevel = Lagrange.Core.Event.EventArg.LogLevel;
using Microsoft.Extensions.Hosting;
using MsLogLevel = Microsoft.Extensions.Logging.LogLevel;
using System;
using Lagrange.Core.Event.EventArg;
using Microsoft.Extensions.Logging;

namespace Lagrange.Kritor.Services.Lagrange.Core;

public class BotLoggerService(ILogger<BotContext> logger, BotContext bot) : IHostedService {
    public Task StartAsync(CancellationToken token) {
        bot.Invoker.OnBotLogEvent += HandleBotLogEvent;

        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken token) {
        bot.Invoker.OnBotLogEvent -= HandleBotLogEvent;

        return Task.CompletedTask;
    }

    private void HandleBotLogEvent(BotContext _, BotLogEvent @event) {
        logger.LogBotContextLog(
            @event.Level switch {
                CoreLogLevel.Debug => MsLogLevel.Debug,
                CoreLogLevel.Verbose => MsLogLevel.Information,
                CoreLogLevel.Information => MsLogLevel.Information,
                CoreLogLevel.Warning => MsLogLevel.Warning,
                CoreLogLevel.Exception => MsLogLevel.Error,
                CoreLogLevel.Fatal => MsLogLevel.Error,
                _ => throw new NotSupportedException($"Not supported LogLevel({@event.Level})"),
            },
            @event.ToString()
        );
    }
}

public static partial class BotContextLogger {
    [LoggerMessage(EventId = 100, Message = "{message}")]
    public static partial void LogBotContextLog(this ILogger<BotContext> logger, MsLogLevel level, string message);
}