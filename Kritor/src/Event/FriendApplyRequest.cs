namespace Kritor.Event;

public partial class FriendApplyRequest {
    public FriendApplyRequest SetApplierUid(string applierUid) {
        ApplierUid = applierUid;
        return this;
    }

    public FriendApplyRequest SetApplierUin(ulong applierUin) {
        ApplierUin = applierUin;
        return this;
    }

    public FriendApplyRequest SetMessage(string message) {
        Message = message;
        return this;
    }
}