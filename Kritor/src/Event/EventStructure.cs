using Kritor.Common;

namespace Kritor.Event;

public partial class EventStructure {
    public EventStructure SetType(EventType type) {
        Type = type;
        return this;
    }
    
    public EventStructure SetMessage(PushMessageBody message) {
        Message = message;
        return this;
    }

    public EventStructure SetRequest(RequestEvent request) {
        Request = request;
        return this;
    }

    public EventStructure SetNotice(NoticeEvent notice) {
        Notice = notice;
        return this;
    }
}