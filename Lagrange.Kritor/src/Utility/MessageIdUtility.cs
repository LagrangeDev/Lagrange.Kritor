using System;

namespace Lagrange.Kritor.Utility;

public static class MessageIdUtility {
    public static string BuildMessageId(long time, uint sequence) {
        return $"{time:D32}_{sequence:D10}_____Lagrange.Kritor_";
    }

    public static string BuildMessageId(DateTimeOffset time, uint sequence) {
        return BuildMessageId(time.ToUnixTimeSeconds(), sequence);
    }

    public static string BuildMessageId(DateTime time, uint sequence) {
        return BuildMessageId(new DateTimeOffset(time).ToUnixTimeSeconds(), sequence);
    }
}