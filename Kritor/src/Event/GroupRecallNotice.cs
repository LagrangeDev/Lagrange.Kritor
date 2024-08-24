namespace Kritor.Event;

public partial class GroupRecallNotice {
    public GroupRecallNotice SetGroupId(ulong groupId) {
        GroupId = groupId;
        return this;
    }

    public GroupRecallNotice SetOperatorUid(string operatorUid) {
        OperatorUid = operatorUid;
        return this;
    }

    public GroupRecallNotice SetOperatorUin(ulong operatorUin) {
        OperatorUin = operatorUin;
        return this;
    }

    public GroupRecallNotice SetTargetUid(string targetUid) {
        TargetUid = targetUid;
        return this;
    }

    public GroupRecallNotice SetTargetUin(ulong targetUin) {
        TargetUin = targetUin;
        return this;
    }

    public GroupRecallNotice SetMessageId(string messageId) {
        MessageId = messageId;
        return this;
    }

    public GroupRecallNotice SetTipText(string tipText) {
        TipText = tipText;
        return this;
    }

}