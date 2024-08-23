namespace Kritor.Common;

public partial class ReplyElement {
    public ReplyElement SetMessageId(string messageId) {
        MessageId = messageId;
        return this;
    }
}