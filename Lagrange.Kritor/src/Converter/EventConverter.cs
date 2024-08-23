using System;
using Kritor.Common;
using Kritor.Event;
using Lagrange.Core.Event.EventArg;

namespace Lagrange.Kritor.Converter;

public static class EventConverter {
    public static EventStructure ToPushMessageBody(this GroupMessageEvent @event) {
        long timestamp = new DateTimeOffset(@event.Chain.Time).ToUnixTimeSeconds();

        string messageId = $"{timestamp:D32}_{@event.Chain.MessageId:D20}_{@event.Chain.Sequence:D10}";

        return EventStructure.CreateGroupMessage(
            (ulong)timestamp,
            messageId,
            @event.Chain.Sequence,
            GroupSender.Create(
                @event.Chain.GroupUin?.ToString() ?? throw new Exception("GroupMessageEvent cannot retrieve GroupUin"),
                @event.Chain.TargetUin,
                @event.Chain.GroupMemberInfo?.MemberName
                    ?? throw new Exception("GroupMessageEvent cannot retrieve GroupMemberInfo.MemberName")
            ),
            @event.Chain.ToElements()
        );
    }
}