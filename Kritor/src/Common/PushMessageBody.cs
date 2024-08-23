namespace Kritor.Common;

public partial class PushMessageBody {
    public PushMessageBody SetTime(ulong time) {
        Time = time;
        return this;
    }

    public PushMessageBody SetMessageId(string messageId) {
        MessageId = messageId;
        return this;
    }

    public PushMessageBody SetMessageSeq(ulong messageSeq) {
        MessageSeq = messageSeq;
        return this;
    }

    public PushMessageBody SetScene(Scene scene) {
        Scene = scene;
        return this;
    }

    public PushMessageBody SetPrivate(PrivateSender @private) {
        Private = @private;
        return this;
    }

    public PushMessageBody SetGroup(GroupSender group) {
        Group = group;
        return this;
    }

    public PushMessageBody SetGuild(GuildSender guild) {
        Guild = guild;
        return this;
    }

    public PushMessageBody AddElements(params Element[] elements) {
        Elements.Add(elements);
        return this;
    }
}