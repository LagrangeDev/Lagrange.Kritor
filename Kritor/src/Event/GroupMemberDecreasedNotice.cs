using static Kritor.Event.GroupMemberDecreasedNotice.Types;
using static Kritor.Event.GroupMemberIncreasedNotice.Types;

namespace Kritor.Event;

public partial class GroupMemberDecreasedNotice {
    public GroupMemberDecreasedNotice SetGroupId(ulong groupId) {
        GroupId = groupId;
        return this;
    }

    public GroupMemberDecreasedNotice SetOperatorUid(string operatorUid) {
        OperatorUid = operatorUid;
        return this;
    }

    public GroupMemberDecreasedNotice SetOperatorUin(ulong operatorUin) {
        OperatorUin = operatorUin;
        return this;
    }

    public GroupMemberDecreasedNotice SetTargetUid(string targetUid) {
        TargetUid = targetUid;
        return this;
    }

    public GroupMemberDecreasedNotice SetTargetUin(ulong targetUin) {
        TargetUin = targetUin;
        return this;
    }

    public GroupMemberDecreasedNotice SetType(GroupMemberDecreasedType type) {
        Type = type;
        return this;
    }

}