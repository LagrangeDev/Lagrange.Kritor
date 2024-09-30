using System;
using Lagrange.Core.Message;

namespace Lagrange.Kritor.Utilities;

public static class MessageIdUtility {
    public static string BuildPrivateMessageId(ulong uin, ulong sequence) {
        return $"p{uin:D20}{sequence:D20}";
    }

    public static string BuildGroupMessageId(ulong uin, ulong sequence) {
        return $"g{uin:D20}{sequence:D20}";
    }

    public static string BuildMessageId(MessageChain chain) {
        return chain.Type switch {
            MessageChain.MessageType.Group => BuildGroupMessageId((ulong)chain.GroupUin!, chain.Sequence),
            MessageChain.MessageType.Temp => throw new NotSupportedException(
                $"Not supported MessageType({MessageChain.MessageType.Temp})"
            ),
            MessageChain.MessageType.Friend => BuildPrivateMessageId(chain.FriendUin, chain.Sequence),
            _ => throw new NotSupportedException($"Not supported MessageChain.MessageType({chain.Type})"),
        };
    }

    public static string BuildMessageId(MessageChain chain, MessageResult result) {
        return chain.Type switch {
            MessageChain.MessageType.Group => BuildGroupMessageId((ulong)chain.GroupUin!, (ulong)result.Sequence!),
            MessageChain.MessageType.Temp => throw new NotSupportedException(
                $"Not supported MessageType({MessageChain.MessageType.Temp})"
            ),
            MessageChain.MessageType.Friend => BuildPrivateMessageId(chain.FriendUin, (ulong)result.Sequence!),
            _ => throw new NotSupportedException($"Not supported MessageChain.MessageType({chain.Type})"),
        };
    }

    public static bool IsGroup(string id) {
        return id.StartsWith('g');
    }

    public static bool IsPrivate(string id) {
        return id.StartsWith('p');
    }

    public static uint GetUin(string id) {
        return uint.Parse(id[1..21]);
    }

    public static uint GetSequence(string id) {
        return uint.Parse(id[21..]);
    }
}