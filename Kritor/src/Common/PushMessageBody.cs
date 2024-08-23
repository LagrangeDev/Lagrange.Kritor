namespace Kritor.Common;

public partial class PushMessageBody {
    public PushMessageBody AddElements(params Element[] elements) {
        Elements.Add(elements);
        return this;
    }

    public static PushMessageBody CreateGroup(ulong time, string messageId, ulong messageSeq, GroupSender group, params Element[] elements) {
        return new PushMessageBody() {
            Time = time,
            MessageId = messageId,
            MessageSeq = messageSeq,
            Scene = Scene.Group,
            Group = group,
        }.AddElements(elements);
    }
}