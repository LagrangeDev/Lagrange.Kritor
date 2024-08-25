using System;

namespace Lagrange.Kritor.Utility;

public static class MessageIdUtility {
    public static string BuildGroupMessageId(ulong groupUin, ulong sequence) {
        return $"g{groupUin:D20}{0:D20}{sequence:D20}";
    }

    public static string BuildPrivateMessageId(ulong uin, long time) {
        return $"p{uin:D20}{time:D20}{0:D20}";
    }

    public static string BuildPrivateMessageId(ulong uin, DateTimeOffset time) {
        return BuildPrivateMessageId(uin, time.ToUnixTimeSeconds());
    }

    public static string BuildPrivateMessageId(ulong uin, DateTime time) {
        return BuildPrivateMessageId(uin, new DateTimeOffset(time));
    }
}