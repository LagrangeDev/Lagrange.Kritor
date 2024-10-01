using System;
using Kritor.Event;
using Lagrange.Core.Event;
using Lagrange.Core.Event.EventArg;

namespace Lagrange.Kritor.Converters;

public static class MessageEventConverter {
    public static EventStructure ToMessageEvent(this EventBase @event) {
        return @event switch {
            FriendMessageEvent message => new EventStructure {
                Type = EventType.Message,
                Message = message.Chain.ToPushMessageBody()
            },
            GroupMessageEvent message => new EventStructure {
                Type = EventType.Message,
                Message = message.Chain.ToPushMessageBody()
            },
            _ => throw new NotSupportedException($"Not supported Event({@event})"),
        };
    }
}