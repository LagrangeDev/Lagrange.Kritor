using static Kritor.Event.GroupMemberIncreasedNotice.Types;

namespace Kritor.Event;

public partial class GroupMemberIncreasedNotice {
    public GroupMemberIncreasedNotice SetGroupId(ulong groupId) {
        GroupId = groupId;
        return this;
    }

    public GroupMemberIncreasedNotice SetOperatorUid(string operatorUid) {
        OperatorUid = operatorUid;
        return this;
    }

    public GroupMemberIncreasedNotice SetOperatorUin(ulong operatorUin) {
        OperatorUin = operatorUin;
        return this;
    }

    public GroupMemberIncreasedNotice SetTargetUid(string targetUid) {
        TargetUid = targetUid;
        return this;
    }

    public GroupMemberIncreasedNotice SetTargetUin(ulong targetUin) {
        TargetUin = targetUin;
        return this;
    }

    public GroupMemberIncreasedNotice SetType(GroupMemberIncreasedType type) {
        Type = type;
        return this;
    }

}