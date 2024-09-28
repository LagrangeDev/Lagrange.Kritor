using System;
using Kritor.Event;
using Lagrange.Core.Event;
using Lagrange.Core.Event.EventArg;
using static Kritor.Event.RequestEvent.Types;

namespace Lagrange.Kritor.Converters;

public static class RequestEventConverter {
    public static EventStructure ToRequestEvent(this EventBase @event) {
        return @event switch {
            FriendRequestEvent request => new EventStructure {
                Type = EventType.Request,
                Request = new RequestEvent {
                    Type = RequestType.FriendApply,
                    Time = (ulong)new DateTimeOffset(request.EventTime).ToUnixTimeSeconds(),
                    FriendApply = new FriendApplyRequest {
                        ApplierUin = request.SourceUin,
                        Message = request.Message
                    }
                }
            },
            GroupJoinRequestEvent request => new EventStructure {
                Type = EventType.Request,
                Request = new RequestEvent {
                    Type = RequestType.GroupApply,
                    Time = (ulong)new DateTimeOffset(request.EventTime).ToUnixTimeSeconds(),
                    GroupApply = new GroupApplyRequest {
                        GroupId = request.GroupUin,
                        ApplierUin = request.TargetUin
                    }
                }
            },
            GroupInvitationRequestEvent request => new EventStructure {
                Type = EventType.Request,
                Request = new RequestEvent {
                    Type = RequestType.InvitedGroup,
                    Time = (ulong)new DateTimeOffset(request.EventTime).ToUnixTimeSeconds(),
                    InvitedGroup = new InvitedJoinGroupRequest {
                        GroupId = request.GroupUin,
                        InviterUin = request.InvitorUin
                    }
                }
            },
            _ => throw new NotImplementedException(),
        };
    }
}