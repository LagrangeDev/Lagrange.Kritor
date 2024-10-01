using System;
using Kritor.Common;
using Kritor.Event;
using Lagrange.Core.Event;
using Lagrange.Core.Event.EventArg;
using Lagrange.Kritor.Utilities;

namespace Lagrange.Kritor.Converters;

public static class MessageEventConverter {
    public static EventStructure ToMessageEvent(this EventBase @event) {
        return @event switch {
            FriendMessageEvent message => new EventStructure {
                Type = EventType.Message,
                Message = new PushMessageBody {
                    Time = (ulong)new DateTimeOffset(message.Chain.Time).ToUnixTimeSeconds(),
                    MessageId = MessageIdUtility.BuildPrivateMessageId(message.Chain.FriendUin, message.Chain.Sequence),
                    MessageSeq = message.Chain.Sequence,
                    Scene = Scene.Friend,
                    Private = new PrivateSender {
                        Uin = message.Chain.FriendUin,
                        Nick = message.Chain.FriendInfo?.Nickname
                        ?? throw new Exception("`FriendMessageEvent.Chain.FriendInfo.Nickname` is null")
                    },
                    Elements = { message.Chain.ToElements() }
                }
            },
            GroupMessageEvent message => new EventStructure {
                Type = EventType.Message,
                Message = new PushMessageBody {
                    Time = (ulong)new DateTimeOffset(message.EventTime).ToUnixTimeSeconds(),
                    MessageId = MessageIdUtility.BuildGroupMessageId(
                        message.Chain.GroupUin ?? throw new Exception("`GroupMessageEvent.Chain.GroupUin` is null"),
                        message.Chain.Sequence
                    ),
                    MessageSeq = message.Chain.Sequence,
                    Scene = Scene.Friend,
                    Group = new GroupSender {
                        GroupId = message.Chain.GroupUin?.ToString()
                        ?? throw new Exception("`GroupMessageEvent.Chain.GroupUin` is null"),
                        Uin = message.Chain.FriendUin,
                        Nick = message.Chain.GroupMemberInfo?.MemberName
                        ?? throw new Exception("`GroupMessageEvent.Chain.GroupMemberInfo.MemberName` is null")
                    },
                    Elements = { message.Chain.ToElements() }
                }
            },
            _ => throw new NotSupportedException($"Not supported Event({@event})"),
        };
    }
}