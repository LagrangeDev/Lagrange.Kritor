namespace Kritor.Event;

public partial class GroupAdminChangedNotice {
    public GroupAdminChangedNotice SetGroupId(ulong groupId) {
        GroupId = groupId;
        return this;
    }

    public GroupAdminChangedNotice SetTargetUid(string targetUid) {
        TargetUid = targetUid;
        return this;
    }

    public GroupAdminChangedNotice SetTargetUin(ulong targetUin) {
        TargetUin = targetUin;
        return this;
    }

    public GroupAdminChangedNotice SetIsAdmin(bool isAdmin) {
        IsAdmin = isAdmin;
        return this;
    }

}