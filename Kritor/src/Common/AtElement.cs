namespace Kritor.Common;

public partial class AtElement {
    public AtElement SetUid(string uid) {
        Uid = uid;
        return this;
    }

    public AtElement SetUin(ulong uin) {
        Uin = uin;
        return this;
    }
}