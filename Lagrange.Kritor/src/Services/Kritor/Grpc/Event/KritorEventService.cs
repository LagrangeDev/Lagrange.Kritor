using System;
using System.Threading;
using System.Threading.Tasks;
using Grpc.Core;
using Kritor.Event;
using Lagrange.Core;
using Lagrange.Kritor.Converters;
using Microsoft.Extensions.Hosting;
using static Kritor.Event.EventService;

namespace Lagrange.Kritor.Services.Kritor.Grpc.Event;

public class KritorEventService : EventServiceBase {
    // private event Action<EventStructure>? OnKritorCoreEvent; // NoEvent

    private event Action<EventStructure>? OnKritorMessageEvent;

    private event Action<EventStructure>? OnKritorNoticeEvent;

    private event Action<EventStructure>? OnKritorRequestEvent;

    private readonly CancellationToken _ct;

    public KritorEventService(BotContext bot, IHostApplicationLifetime lifetime) {
        _ct = lifetime.ApplicationStopping;

        // CoreEvent

        // MessageEvent
        bot.Invoker.OnFriendMessageReceived += (_, e) => OnKritorMessageEvent?.Invoke(e.ToMessageEvent());
        bot.Invoker.OnGroupMessageReceived += (_, e) => OnKritorMessageEvent?.Invoke(e.ToMessageEvent());

        // NoticeEvent
        bot.Invoker.OnFriendPokeEvent += async (_, e) => {
            OnKritorNoticeEvent?.Invoke(await e.ToNoticeEvent(bot, lifetime.ApplicationStopping));
        };
        bot.Invoker.OnFriendRecallEvent += async (_, e) => {
            OnKritorNoticeEvent?.Invoke(await e.ToNoticeEvent(bot, lifetime.ApplicationStopping));
        };
        // bot.Invoker.On // TODO: PrivateFileUploadedNotice
        bot.Invoker.OnGroupPokeEvent += async (_, e) => {
            OnKritorNoticeEvent?.Invoke(await e.ToNoticeEvent(bot, lifetime.ApplicationStopping));
        };
        bot.Invoker.OnGroupRecallEvent += async (_, e) => {
            OnKritorNoticeEvent?.Invoke(await e.ToNoticeEvent(bot, lifetime.ApplicationStopping));
        };
        // bot.Invoker.On // TODO: GroupFileUploadedNotice
        // bot.Invoker.On // TODO: GroupCardChangedNotice
        // bot.Invoker.On // TODO: GroupUniqueTitleChangedNotice
        bot.Invoker.OnGroupEssenceEvent += async (_, e) => {
            OnKritorNoticeEvent?.Invoke(await e.ToNoticeEvent(bot, lifetime.ApplicationStopping));
        };
        bot.Invoker.OnGroupMemberIncreaseEvent += async (_, e) => {
            OnKritorNoticeEvent?.Invoke(await e.ToNoticeEvent(bot, lifetime.ApplicationStopping));
        };
        bot.Invoker.OnGroupMemberDecreaseEvent += async (_, e) => {
            OnKritorNoticeEvent?.Invoke(await e.ToNoticeEvent(bot, lifetime.ApplicationStopping));
        };
        bot.Invoker.OnGroupAdminChangedEvent += async (_, e) => {
            OnKritorNoticeEvent?.Invoke(await e.ToNoticeEvent(bot, lifetime.ApplicationStopping));
        };
        // bot.Invoker.On // TODO: GroupSignInNotice
        bot.Invoker.OnGroupMemberMuteEvent += async (_, e) => {
            OnKritorNoticeEvent?.Invoke(await e.ToNoticeEvent(bot, lifetime.ApplicationStopping));
        };
        bot.Invoker.OnGroupMuteEvent += async (_, e) => {
            OnKritorNoticeEvent?.Invoke(await e.ToNoticeEvent(bot, lifetime.ApplicationStopping));
        };
        bot.Invoker.OnGroupReactionEvent += async (_, e) => {
            OnKritorNoticeEvent?.Invoke(await e.ToNoticeEvent(bot, lifetime.ApplicationStopping));
        };
        // bot.Invoker.On // TODO: GroupTransferNotice
        // bot.Invoker.On // TODO: FriendIncreasedNotice
        // bot.Invoker.On // TODO: FriendDecreasedNotice

        // RequestEvent
        bot.Invoker.OnFriendRequestEvent += (_, e) => OnKritorRequestEvent?.Invoke(e.ToRequestEvent());
        bot.Invoker.OnGroupJoinRequestEvent += (_, e) => OnKritorRequestEvent?.Invoke(e.ToRequestEvent());
        bot.Invoker.OnGroupInvitationRequestEvent += (_, e) => OnKritorRequestEvent?.Invoke(e.ToRequestEvent());
    }

    private static Action<EventStructure> CreateKritorEventHandler(IServerStreamWriter<EventStructure> stream, TaskCompletionSource tcs, CancellationToken token) {
        return async (@event) => {
            try {
                await stream.WriteAsync(@event, token);
            } catch (Exception e) {
                tcs.SetException(e);
            }
        };
    }

    public override async Task RegisterActiveListener(RequestPushEvent request, IServerStreamWriter<EventStructure> responseStream, ServerCallContext context) {
        TaskCompletionSource tcs = new();

        Action<EventStructure> handler = CreateKritorEventHandler(responseStream, tcs, context.CancellationToken);
        switch (request.Type) {
            // case EventType.CoreEvent: { OnKritorCoreEvent += handler; break; }
            case EventType.Message: { OnKritorMessageEvent += handler; break; }
            case EventType.Notice: { OnKritorNoticeEvent += handler; break; }
            case EventType.Request: { OnKritorRequestEvent += handler; break; }
        }

        CancellationTokenSource.CreateLinkedTokenSource(_ct, context.CancellationToken).Token.Register(tcs.SetResult);
        try {
            await tcs.Task;
        } finally {
            switch (request.Type) {
                // case EventType.CoreEvent: { OnKritorCoreEvent -= handler; break; }
                case EventType.Message: { OnKritorMessageEvent -= handler; break; }
                case EventType.Notice: { OnKritorNoticeEvent -= handler; break; }
                case EventType.Request: { OnKritorRequestEvent -= handler; break; }
            }
        }
    }

    public override Task<RequestPushEvent> RegisterPassiveListener(IAsyncStreamReader<EventStructure> requestStream, ServerCallContext context) {
        return base.RegisterPassiveListener(requestStream, context);
    }
}
