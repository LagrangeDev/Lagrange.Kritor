using System;
using Kritor.Event;
using Lagrange.Core.Event.EventArg;
using static Kritor.Event.RequestEvent.Types;

namespace Lagrange.Kritor.Converter;

public static class RequestEventConverter {
    public static EventStructure ToRequestEvent(this FriendRequestEvent @event) {
        return new EventStructure()
            .SetType(EventType.Request)
            .SetRequest(new RequestEvent()
                .SetType(RequestType.FriendApply)
                .SetTime((ulong)new DateTimeOffset(@event.EventTime).ToUnixTimeSeconds())
                .SetFriendApply(new FriendApplyRequest()
                    .SetApplierUin(@event.SourceUin)
                    .SetMessage(@event.Message)
                )
            );
    }

    public static EventStructure ToRequestEvent(this GroupJoinRequestEvent @event) {
        return new EventStructure()
            .SetType(EventType.Request)
            .SetRequest(new RequestEvent()
                .SetType(RequestType.GroupApply)
                .SetTime((ulong)new DateTimeOffset(@event.EventTime).ToUnixTimeSeconds())
                .SetGroupApply(new GroupApplyRequest()
                    .SetGroupId(@event.GroupUin)
                    .SetApplierUin(@event.TargetUin)
                    // .SetInviterUin(@event.) // TODO: Lagrange NotSupport // TODO: optional
                    // .SetReason(@event.) // TODO: Lagrange NotSupport
                )
            );
    }

    public static EventStructure ToRequestEvent(this GroupInvitationRequestEvent @event) {
        return new EventStructure()
            .SetType(EventType.Request)
            .SetRequest(new RequestEvent()
                .SetType(RequestType.InvitedGroup)
                .SetTime((ulong)new DateTimeOffset(@event.EventTime).ToUnixTimeSeconds())
                .SetInvitedGroup(new InvitedJoinGroupRequest()
                    .SetGroupId(@event.GroupUin)
                    .SetInviterUin(@event.InvitorUin)
                )
            );
    }
}