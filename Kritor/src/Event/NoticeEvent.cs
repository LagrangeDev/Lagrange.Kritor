using static Kritor.Event.NoticeEvent.Types;

namespace Kritor.Event;

public partial class NoticeEvent {
    public NoticeEvent SetType(NoticeType type) {
        Type = type;
        return this;
    }

    public NoticeEvent SetTime(ulong time) {
        Time = time;
        return this;
    }

    public NoticeEvent SetNoticeId(string noticeId) {
        NoticeId = noticeId;
        return this;
    }

    public NoticeEvent SetPrivatePoke(PrivatePokeNotice privatePoke) {
        PrivatePoke = privatePoke;
        return this;
    }

    public NoticeEvent SetPrivateRecall(PrivateRecallNotice privateRecall) {
        PrivateRecall = privateRecall;
        return this;
    }

    public NoticeEvent SetPrivateFileUploaded(PrivateFileUploadedNotice privateFileUploaded) {
        PrivateFileUploaded = privateFileUploaded;
        return this;
    }

    public NoticeEvent SetGroupPoke(GroupPokeNotice groupPoke) {
        GroupPoke = groupPoke;
        return this;
    }

    public NoticeEvent SetGroupRecall(GroupRecallNotice groupRecall) {
        GroupRecall = groupRecall;
        return this;
    }

    public NoticeEvent SetGroupFileUploaded(GroupFileUploadedNotice groupFileUploaded) {
        GroupFileUploaded = groupFileUploaded;
        return this;
    }

    public NoticeEvent SetGroupCardChanged(GroupCardChangedNotice groupCardChanged) {
        GroupCardChanged = groupCardChanged;
        return this;
    }

    public NoticeEvent SetGroupMemberUniqueTitleChanged(GroupUniqueTitleChangedNotice groupMemberUniqueTitleChanged) {
        GroupMemberUniqueTitleChanged = groupMemberUniqueTitleChanged;
        return this;
    }

    public NoticeEvent SetGroupEssenceChanged(GroupEssenceMessageNotice groupEssenceChanged) {
        GroupEssenceChanged = groupEssenceChanged;
        return this;
    }

    public NoticeEvent SetGroupMemberIncrease(GroupMemberIncreasedNotice groupMemberIncrease) {
        GroupMemberIncrease = groupMemberIncrease;
        return this;
    }

    public NoticeEvent SetGroupMemberDecrease(GroupMemberDecreasedNotice groupMemberDecrease) {
        GroupMemberDecrease = groupMemberDecrease;
        return this;
    }

    public NoticeEvent SetGroupAdminChanged(GroupAdminChangedNotice groupAdminChanged) {
        GroupAdminChanged = groupAdminChanged;
        return this;
    }

    public NoticeEvent SetGroupSignIn(GroupSignInNotice groupSignIn) {
        GroupSignIn = groupSignIn;
        return this;
    }

    public NoticeEvent SetGroupMemberBan(GroupMemberBanNotice groupMemberBan) {
        GroupMemberBan = groupMemberBan;
        return this;
    }

    public NoticeEvent SetGroupWholeBan(GroupWholeBanNotice groupWholeBan) {
        GroupWholeBan = groupWholeBan;
        return this;
    }

    public NoticeEvent SetGroupReactMessageWithEmoji(GroupReactMessageWithEmojiNotice groupReactMessageWithEmoji) {
        GroupReactMessageWithEmoji = groupReactMessageWithEmoji;
        return this;
    }

    public NoticeEvent SetGroupTransfer(GroupTransferNotice groupTransfer) {
        GroupTransfer = groupTransfer;
        return this;
    }

    public NoticeEvent SetFriendIncrease(FriendIncreasedNotice friendIncrease) {
        FriendIncrease = friendIncrease;
        return this;
    }

    public NoticeEvent SetFriendDecrease(FriendDecreasedNotice friendDecrease) {
        FriendDecrease = friendDecrease;
        return this;
    }
}