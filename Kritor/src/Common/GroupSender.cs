namespace Kritor.Common;

public partial class GroupSender {
    public GroupSender SetGroupId(string groupId) {
        GroupId = groupId;
        return this;
    }

    public GroupSender SetUid(string uid) {
        Uid = uid;
        return this;
    }

    public GroupSender SetUin(uint uin) {
        Uin = uin;
        return this;
    }

    public GroupSender SetNick(string nick) {
        Nick = nick;
        return this;
    }
}