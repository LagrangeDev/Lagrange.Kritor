namespace Kritor.Event;

public partial class GroupReactMessageWithEmojiNotice {
    public GroupReactMessageWithEmojiNotice SetGroupId(ulong groupId) {
        GroupId = groupId;
        return this;
    }

    public GroupReactMessageWithEmojiNotice SetMessageId(string messageId) {
        MessageId = messageId;
        return this;
    }

    public GroupReactMessageWithEmojiNotice SetFaceId(uint faceId) {
        FaceId = faceId;
        return this;
    }

    public GroupReactMessageWithEmojiNotice SetIsSet(bool isSet) {
        IsSet = isSet;
        return this;
    }

}