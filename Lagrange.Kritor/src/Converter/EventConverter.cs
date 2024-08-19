using System;
using Kritor.Common;
using Kritor.Event;
using Lagrange.Core.Event;
using Lagrange.Core.Event.EventArg;

namespace Lagrange.Kritor.Converter;

public static class EventConverter {
    public static EventStructure ToPushMessageBody(this GroupMessageEvent @event) {
        long timestamp = new DateTimeOffset(@event.Chain.Time).ToUnixTimeSeconds();

        string messageId = $"{timestamp:D32}_{@event.Chain.MessageId:D20}_{@event.Chain.Sequence:D10}";

        return EventStructure.CreateMessage(
            (ulong)timestamp,
            messageId,
            @event.Chain.Sequence,
            Contact.CreateGroup(@event.Chain.GroupUin?.ToString()
                ?? throw new Exception("GroupMessageEvent does not have GroupUin")),
            Sender.Create(@event.Chain.FriendUin),
            @event.Chain.ToElements()
        );
    }
}