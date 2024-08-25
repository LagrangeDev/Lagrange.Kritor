namespace Kritor.Event;

public partial class GroupApplyRequest {
    public GroupApplyRequest SetGroupId(ulong groupId) {
        GroupId = groupId;
        return this;
    }

    public GroupApplyRequest SetApplierUid(string applierUid) {
        ApplierUid = applierUid;
        return this;
    }

    public GroupApplyRequest SetApplierUin(ulong applierUin) {
        ApplierUin = applierUin;
        return this;
    }

    public GroupApplyRequest SetInviterUid(string inviterUid) {
        InviterUid = inviterUid;
        return this;
    }

    public GroupApplyRequest SetInviterUin(ulong inviterUin) {
        InviterUin = inviterUin;
        return this;
    }

    public GroupApplyRequest SetReason(string reason) {
        Reason = reason;
        return this;
    }
}