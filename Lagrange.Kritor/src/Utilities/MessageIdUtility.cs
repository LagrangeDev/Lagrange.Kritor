namespace Lagrange.Kritor.Utilities;

public static class MessageIdUtility {
    public static string BuildGroupMessageId(ulong uin, ulong sequence) {
        return $"g{uin:D20}{0:D20}{sequence:D20}";
    }

    public static string BuildPrivateMessageId(ulong uin, ulong sequence) {
        return $"p{uin:D20}{sequence:D20}{0:D20}";
    }
}