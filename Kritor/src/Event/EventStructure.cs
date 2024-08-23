using Kritor.Common;

namespace Kritor.Event;

public partial class EventStructure {
    private EventStructure(EventType type) {
        Type = type;
    }

    public static EventStructure CreateGroupMessage(ulong time, string messageId, ulong messageSeq, GroupSender group, params Element[] elements) {
        return new(EventType.Message) {
            Message = PushMessageBody.CreateGroup(time, messageId, messageSeq, group, elements)
        };
    }
}