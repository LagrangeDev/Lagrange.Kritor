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
        bot.Invoker.OnFriendMessageReceived += (_, e) => OnKritorMessageEvent?.Invoke(e.ToPushMessageBody());
        bot.Invoker.OnGroupMessageReceived += (_, e) => OnKritorMessageEvent?.Invoke(e.ToPushMessageBody());

        // NoticeEvent
        bot.Invoker.OnFriendPokeEvent += (_, e) => OnKritorNoticeEvent?.Invoke(e.ToNoticeEvent());
        bot.Invoker.OnFriendRecallEvent += (_, e) => OnKritorNoticeEvent?.Invoke(e.ToNoticeEvent());
        // bot.Invoker.On // TODO: PrivateFileUploadedNotice
        bot.Invoker.OnGroupPokeEvent += (_, e) => OnKritorNoticeEvent?.Invoke(e.ToNoticeEvent());
        bot.Invoker.OnGroupRecallEvent += (_, e) => OnKritorNoticeEvent?.Invoke(e.ToNoticeEvent());
        // bot.Invoker.On // TODO: GroupFileUploadedNotice
        // bot.Invoker.On // TODO: GroupCardChangedNotice
        // bot.Invoker.On // TODO: GroupUniqueTitleChangedNotice
        bot.Invoker.OnGroupEssenceEvent += (_, e) => OnKritorNoticeEvent?.Invoke(e.ToNoticeEvent());
        bot.Invoker.OnGroupMemberIncreaseEvent += (_, e) => OnKritorNoticeEvent?.Invoke(e.ToNoticeEvent());
        bot.Invoker.OnGroupMemberDecreaseEvent += (_, e) => OnKritorNoticeEvent?.Invoke(e.ToNoticeEvent());
        bot.Invoker.OnGroupAdminChangedEvent += (_, e) => OnKritorNoticeEvent?.Invoke(e.ToNoticeEvent());
        // bot.Invoker.On // TODO: GroupSignInNotice
        bot.Invoker.OnGroupMemberMuteEvent += (_, e) => OnKritorNoticeEvent?.Invoke(e.ToNoticeEvent());
        bot.Invoker.OnGroupMuteEvent += (_, e) => OnKritorNoticeEvent?.Invoke(e.ToNoticeEvent());
        bot.Invoker.OnGroupReactionEvent += (_, e) => OnKritorNoticeEvent?.Invoke(e.ToNoticeEvent());
        // bot.Invoker.On // TODO: GroupTransferNotice
        // bot.Invoker.On // TODO: FriendIncreasedNotice
        // bot.Invoker.On // TODO: FriendDecreasedNotice

        // RequestEvent
        bot.Invoker.OnFriendRequestEvent += (_, e) => OnKritorRequestEvent?.Invoke(e.ToRequestEvent());
        bot.Invoker.OnGroupJoinRequestEvent += (_, e) => OnKritorRequestEvent?.Invoke(e.ToRequestEvent());
        bot.Invoker.OnGroupInvitationRequestEvent += (_, e) => OnKritorRequestEvent?.Invoke(e.ToRequestEvent());
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
