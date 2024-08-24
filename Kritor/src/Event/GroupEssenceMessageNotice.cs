namespace Kritor.Event;

public partial class GroupEssenceMessageNotice {
    public GroupEssenceMessageNotice SetGroupId(ulong groupId) {
        GroupId = groupId;
        return this;
    }

    public GroupEssenceMessageNotice SetOperatorUid(string operatorUid) {
        OperatorUid = operatorUid;
        return this;
    }

    public GroupEssenceMessageNotice SetOperatorUin(ulong operatorUin) {
        OperatorUin = operatorUin;
        return this;
    }

    public GroupEssenceMessageNotice SetTargetUid(string targetUid) {
        TargetUid = targetUid;
        return this;
    }

    public GroupEssenceMessageNotice SetTargetUin(ulong targetUin) {
        TargetUin = targetUin;
        return this;
    }

    public GroupEssenceMessageNotice SetMessageId(string messageId) {
        MessageId = messageId;
        return this;
    }

    public GroupEssenceMessageNotice SetIsSet(bool isSet) {
        IsSet = isSet;
        return this;
    }
}