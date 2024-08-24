using System;
using System.Threading;
using System.Threading.Tasks;
using Grpc.Core;
using Kritor.Event;
using Lagrange.Core;
using Lagrange.Kritor.Converter;
using static Kritor.Event.EventService;

namespace Lagrange.Kritor.Service.Kritor.Grpc.Event;

public class KritorEventService : EventServiceBase {
    public event Func<EventStructure, Task>? OnKritorCoreEvent;

    public event Func<EventStructure, Task>? OnKritorMessageEvent;

    public event Func<EventStructure, Task>? OnKritorNoticeEvent;

    public event Func<EventStructure, Task>? OnKritorRequestEvent;

    public KritorEventService(BotContext bot) {
        // CoreEvent

        // MessageEvent
        bot.Invoker.OnFriendMessageReceived += (_, @event) => {
            OnKritorMessageEvent?.Invoke(EventConverter.ToPushMessageBody(@event));
        };
        bot.Invoker.OnGroupMessageReceived += (_, @event) => {
            OnKritorMessageEvent?.Invoke(EventConverter.ToPushMessageBody(@event));
        };
    }

    public override async Task RegisterActiveListener(RequestPushEvent request, IServerStreamWriter<EventStructure> responseStream, ServerCallContext context) {
        TaskCompletionSource tcs = new();

        Func<EventStructure, Task> handler = CreateKritorEventHandler(responseStream, tcs, context.CancellationToken);
        switch (request.Type) {
            case EventType.CoreEvent: { OnKritorCoreEvent += handler; break; }
            case EventType.Message: { OnKritorMessageEvent += handler; break; }
            case EventType.Notice: { OnKritorNoticeEvent += handler; break; }
            case EventType.Request: { OnKritorRequestEvent += handler; break; }
        };

        context.CancellationToken.Register(tcs.SetResult);
        try {
            await tcs.Task;
        } catch (Exception e) {
            Console.WriteLine(e);
        } finally {
            switch (request.Type) {
                case EventType.CoreEvent: { OnKritorCoreEvent -= handler; break; }
                case EventType.Message: { OnKritorMessageEvent -= handler; break; }
                case EventType.Notice: { OnKritorNoticeEvent -= handler; break; }
                case EventType.Request: { OnKritorRequestEvent -= handler; break; }
            };
        }
    }

    private static Func<EventStructure, Task> CreateKritorEventHandler(IServerStreamWriter<EventStructure> stream, TaskCompletionSource tcs, CancellationToken token) {
        return async (@event) => {
            try {
                await stream.WriteAsync(@event, token);
            } catch (Exception e) {
                tcs.SetException(e);
            }
        };
    }
}