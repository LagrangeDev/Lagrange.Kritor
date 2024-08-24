namespace Kritor.Event;

public partial class PrivateRecallNotice {
    public PrivateRecallNotice SetOperatorUid(string operatorUid) {
        OperatorUid = operatorUid;
        return this;
    }

    public PrivateRecallNotice SetOperatorUin(ulong operatorUin) {
        OperatorUin = operatorUin;
        return this;
    }

    public PrivateRecallNotice SetMessageId(string messageId) {
        MessageId = messageId;
        return this;
    }

    public PrivateRecallNotice SetTipText(string tipText) {
        TipText = tipText;
        return this;
    }
}