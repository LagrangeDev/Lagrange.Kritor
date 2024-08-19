using Kritor.Common;

namespace Kritor.Event;

public partial class EventStructure {
    private EventStructure(EventType type) {
        Type = type;
    }

    public static EventStructure CreateMessage(ulong time, string messageId, ulong messageSeq, Contact contact, Sender sender, params Element[] elements) {
        return new(EventType.Message) {
            Message = PushMessageBody.Create(time, messageId, messageSeq, contact, sender, elements)
        };
    }

    public static EventStructure CreateRequest(RequestEvent request) {
        return new(EventType.Request) { Request = request };
    }

    public static EventStructure CreateNotice(NoticeEvent notice) {
        return new(EventType.Notice) { Notice = notice };
    }
}