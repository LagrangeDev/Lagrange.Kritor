using static Kritor.Event.RequestEvent.Types;

namespace Kritor.Event;

public partial class RequestEvent {
    public RequestEvent SetType(RequestType type) {
        Type = type;
        return this;
    }

    public RequestEvent SetTime(ulong time) {
        Time = time;
        return this;
    }

    public RequestEvent SetRequestId(string requestId) {
        RequestId = requestId;
        return this;
    }

    public RequestEvent SetFriendApply(FriendApplyRequest friendApply) {
        FriendApply = friendApply;
        return this;
    }

    public RequestEvent SetGroupApply(GroupApplyRequest groupApply) {
        GroupApply = groupApply;
        return this;
    }

    public RequestEvent SetInvitedGroup(InvitedJoinGroupRequest invitedGroup) {
        InvitedGroup = invitedGroup;
        return this;
    }
}