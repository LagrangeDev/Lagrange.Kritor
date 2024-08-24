using static Kritor.Event.GroupMemberBanNotice.Types;

namespace Kritor.Event;

public partial class GroupMemberBanNotice {
    public GroupMemberBanNotice SetGroupId(ulong groupId) {
        GroupId = groupId;
        return this;
    }

    public GroupMemberBanNotice SetOperatorUid(string operatorUid) {
        OperatorUid = operatorUid;
        return this;
    }

    public GroupMemberBanNotice SetOperatorUin(ulong operatorUin) {
        OperatorUin = operatorUin;
        return this;
    }

    public GroupMemberBanNotice SetTargetUid(string targetUid) {
        TargetUid = targetUid;
        return this;
    }

    public GroupMemberBanNotice SetTargetUin(ulong targetUin) {
        TargetUin = targetUin;
        return this;
    }

    public GroupMemberBanNotice SetDuration(int duration) {
        Duration = duration;
        return this;
    }

    public GroupMemberBanNotice SetType(GroupMemberBanType type) {
        Type = type;
        return this;
    }

}