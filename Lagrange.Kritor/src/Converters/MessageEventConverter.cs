using System;
using Kritor.Common;
using Kritor.Event;
using Lagrange.Core.Event.EventArg;
using Lagrange.Kritor.Utilities;

namespace Lagrange.Kritor.Converters;

public static class MessageEventConverter {
    public static EventStructure ToPushMessageBody(this FriendMessageEvent @event) {
        return new EventStructure {
            Type = EventType.Message,
            Message = new PushMessageBody {
                Time = (ulong)new DateTimeOffset(@event.Chain.Time).ToUnixTimeSeconds(),
                MessageId = MessageIdUtility.BuildPrivateMessageId(@event.Chain.FriendUin, @event.Chain.Sequence),
                MessageSeq = @event.Chain.Sequence,
                Private = new PrivateSender {
                    Uin = @event.Chain.FriendUin,
                    Nick = @event.Chain.FriendInfo?.Nickname
                        ?? throw new Exception("`FriendMessageEvent.Chain.FriendInfo.Nickname` is null")
                },
                Elements = { @event.Chain.ToElements() }
            }
        };
    }

    public static EventStructure ToPushMessageBody(this GroupMessageEvent @event) {
        return new EventStructure {
            Type = EventType.Message,
            Message = new PushMessageBody {
                Time = (ulong)new DateTimeOffset(@event.EventTime).ToUnixTimeSeconds(),
                MessageId = MessageIdUtility.BuildGroupMessageId(
                    @event.Chain.GroupUin ?? throw new Exception("`GroupMessageEvent.Chain.GroupUin` is null"),
                    @event.Chain.Sequence
                ),
                MessageSeq = @event.Chain.Sequence,
                Group = new GroupSender {
                    GroupId = @event.Chain.GroupUin?.ToString()
                        ?? throw new Exception("`GroupMessageEvent.Chain.GroupUin` is null"),
                    Uin = @event.Chain.FriendUin,
                    Nick = @event.Chain.GroupMemberInfo?.MemberName
                        ?? throw new Exception("`GroupMessageEvent.Chain.GroupMemberInfo.MemberName` is null")
                },
                Elements = { @event.Chain.ToElements() }
            }
        };
    }
}