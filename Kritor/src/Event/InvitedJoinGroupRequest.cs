namespace Kritor.Event;

public partial class InvitedJoinGroupRequest {
    public InvitedJoinGroupRequest SetGroupId(ulong groupId) {
        GroupId = groupId;
        return this;
    }

    public InvitedJoinGroupRequest SetInviterUid(string inviterUid) {
        InviterUid = inviterUid;
        return this;
    }

    public InvitedJoinGroupRequest SetInviterUin(ulong inviterUin) {
        InviterUin = inviterUin;
        return this;
    }
}