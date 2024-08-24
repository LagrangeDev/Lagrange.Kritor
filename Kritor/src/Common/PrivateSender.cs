namespace Kritor.Common;

public partial class PrivateSender {
    public PrivateSender SetUid(string uid) {
        Uid = uid;
        return this;
    }

    public PrivateSender SetUin(ulong uin) {
        Uin = uin;
        return this;
    }

    public PrivateSender SetNick(string nick) {
        Nick = nick;
        return this;
    }
}