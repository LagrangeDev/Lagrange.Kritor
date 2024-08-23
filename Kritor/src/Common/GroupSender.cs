namespace Kritor.Common;

public partial class GroupSender {
    public static GroupSender Create(string groupId, ulong uin, string nick) {
        return new() { GroupId = groupId, Uin = uin, Nick = nick };
    }
}