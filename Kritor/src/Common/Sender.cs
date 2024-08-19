namespace Kritor.Common;

public partial class Sender {
    private Sender(ulong uin, string? uid, string? nick, Role? role) {
        Uin = uin;
        if (uid != null) Uid = uid;
        if (nick != null) Nick = nick;
        if (role.HasValue) Role = role.Value;
    }

    public static Sender Create(ulong uin, string? uid = null, string? nick = null, Role? role = null) {
        return new(uin, uid, nick, role);
    }
}