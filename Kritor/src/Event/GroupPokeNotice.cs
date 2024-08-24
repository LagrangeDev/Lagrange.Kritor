namespace Kritor.Event;

public partial class GroupPokeNotice {
    public GroupPokeNotice SetGroupId(ulong groupId) {
        GroupId = groupId;
        return this;
    }

    public GroupPokeNotice SetOperatorUid(string operatorUid) {
        OperatorUid = operatorUid;
        return this;
    }

    public GroupPokeNotice SetOperatorUin(ulong operatorUin) {
        OperatorUin = operatorUin;
        return this;
    }
    
    public GroupPokeNotice SetTargetUid(string targetUid) {
        TargetUid = targetUid;
        return this;
    }

    public GroupPokeNotice SetTargetUin(ulong targetUin) {
        TargetUin = targetUin;
        return this;
    }

    public GroupPokeNotice SetAction(string action) {
        Action = action;
        return this;
    }

    public GroupPokeNotice SetSuffix(string suffix) {
        Suffix = suffix;
        return this;
    }
    
    public GroupPokeNotice SetActionImage(string actionImage) {
        ActionImage = actionImage;
        return this;
    }
}