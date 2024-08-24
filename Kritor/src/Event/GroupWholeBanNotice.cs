namespace Kritor.Event;

public partial class GroupWholeBanNotice {
    public GroupWholeBanNotice SetGroupId(ulong groupId) {
        GroupId = groupId;
        return this;
    }

    public GroupWholeBanNotice SetOperatorUid(string operatorUid) {
        OperatorUid = operatorUid;
        return this;
    }

    public GroupWholeBanNotice SetOperatorUin(ulong operatorUin) {
        OperatorUin = operatorUin;
        return this;
    }

    public GroupWholeBanNotice SetIsBan(bool isBan) {
        IsBan = isBan;
        return this;
    }
}