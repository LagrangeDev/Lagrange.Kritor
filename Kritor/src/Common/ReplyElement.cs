namespace Kritor.Common;

public partial class ReplyElement {
    private ReplyElement(string messageId) {
        MessageId = messageId;
    }

    public static ReplyElement Create(string messageId) {
        return new(messageId);
    }
}