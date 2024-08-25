using System;
using Kritor.Common;
using Kritor.Event;
using Lagrange.Core.Event.EventArg;
using Lagrange.Kritor.Utility;
using static Kritor.Event.GroupMemberBanNotice.Types;
using static Kritor.Event.GroupMemberDecreasedNotice.Types;
using static Kritor.Event.GroupMemberIncreasedNotice.Types;
using static Kritor.Event.NoticeEvent.Types;

namespace Lagrange.Kritor.Converter;

public static class MessageEventConverter {
    public static EventStructure ToPushMessageBody(this FriendMessageEvent @event) {
        long timestamp = new DateTimeOffset(@event.Chain.Time).ToUnixTimeSeconds();

        string messageId = MessageIdUtility.BuildPrivateMessageId((ulong)timestamp, @event.Chain.Sequence);

        string nick = @event.Chain.FriendInfo?.Nickname
            ?? throw new Exception("FriendMessageEvent cannot retrieve FriendInfo.MemberName");

        return new EventStructure()
            .SetType(EventType.Message)
            .SetMessage(new PushMessageBody()
                .SetTime((ulong)timestamp)
                .SetMessageId(messageId)
                .SetMessageSeq(@event.Chain.Sequence)
                .SetPrivate(new PrivateSender()
                    .SetUin(@event.Chain.FriendUin)
                    .SetNick(nick)
                )
                .AddElements(@event.Chain.ToElements())
            );
    }

    public static EventStructure ToPushMessageBody(this GroupMessageEvent @event) {
        long timestamp = new DateTimeOffset(@event.EventTime).ToUnixTimeSeconds();

        string messageId = MessageIdUtility.BuildGroupMessageId(
            @event.Chain.GroupUin ?? throw new Exception("GroupMessageEvent cannot retrieve GroupUin"), 
            @event.Chain.Sequence
        );

        string groupId = @event.Chain.GroupUin?.ToString()
            ?? throw new Exception("GroupMessageEvent cannot retrieve GroupUin");

        string nick = @event.Chain.GroupMemberInfo?.MemberName
            ?? throw new Exception("GroupMessageEvent cannot retrieve GroupMemberInfo.MemberName");

        return new EventStructure()
            .SetType(EventType.Message)
            .SetMessage(new PushMessageBody()
                .SetTime((ulong)timestamp)
                .SetMessageId(messageId)
                .SetMessageSeq(@event.Chain.Sequence)
                .SetGroup(new GroupSender()
                    .SetGroupId(groupId)
                    .SetUin(@event.Chain.TargetUin)
                    .SetNick(nick)
                )
                .AddElements(@event.Chain.ToElements())
            );
    }
}