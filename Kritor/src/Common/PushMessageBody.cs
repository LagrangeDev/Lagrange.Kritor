namespace Kritor.Common;

public partial class PushMessageBody {
    private PushMessageBody(ulong time, string messageId, ulong messageSeq, Contact contact, Sender sender, Element[] elements) {
        Time = time;
        MessageId = messageId;
        MessageSeq = messageSeq;
        Contact = contact;
        Sender = sender;
        Elements.Add(elements);
    }

    public static PushMessageBody Create(ulong time, string messageId, ulong messageSeq, Contact contact, Sender sender, params Element[] elements) {
        return new(time, messageId, messageSeq, contact, sender, elements);
    }
}