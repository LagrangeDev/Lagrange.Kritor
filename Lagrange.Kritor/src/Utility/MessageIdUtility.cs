using System;

namespace Lagrange.Kritor.Utility;

public static class MessageIdUtility {
    public static string BuildMessageId(long time, ulong random, uint sequence) {
        return $"{time:D32}_{random:D20}_{sequence:D10}";
    }

    public static string BuildMessageId(DateTimeOffset time, ulong random, uint sequence) {
        return BuildMessageId(time.ToUnixTimeSeconds(), random, sequence);
    }

    public static string BuildMessageId(DateTime time, ulong random, uint sequence) {
        return BuildMessageId(new DateTimeOffset(time).ToUnixTimeSeconds(), random, sequence);
    }
}