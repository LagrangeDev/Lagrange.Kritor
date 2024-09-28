namespace Lagrange.Kritor.Utilities;

public static class MessageIdUtility {
    public static string BuildGroupMessageId(ulong uin, ulong sequence) {
        return $"g{uin:D20}{sequence:D20}";
    }

    public static string BuildPrivateMessageId(ulong uin, ulong sequence) {
        return $"p{uin:D20}{sequence:D20}";
    }

    public static uint GetSequence(string id) {
        return uint.Parse(id[21..]);
    }
}