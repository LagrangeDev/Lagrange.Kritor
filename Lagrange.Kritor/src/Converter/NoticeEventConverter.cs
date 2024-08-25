using System;
using Kritor.Event;
using Lagrange.Core.Event.EventArg;
using Lagrange.Kritor.Utility;
using static Kritor.Event.GroupMemberBanNotice.Types;
using static Kritor.Event.GroupMemberDecreasedNotice.Types;
using static Kritor.Event.GroupMemberIncreasedNotice.Types;
using static Kritor.Event.NoticeEvent.Types;

namespace Lagrange.Kritor.Converter;

public static class NoticeEventConverter {
    public static EventStructure ToNoticeEvent(this FriendPokeEvent @event) {
        return new EventStructure()
            .SetType(EventType.Notice)
            .SetNotice(new NoticeEvent()
                .SetType(NoticeType.PrivatePoke)
                .SetTime((ulong)new DateTimeOffset(@event.EventTime).ToUnixTimeSeconds())
                .SetNoticeId(Guid.NewGuid().ToString())
                .SetPrivatePoke(new PrivatePokeNotice()
                    .SetOperatorUin(@event.OperatorUin)
                    .SetAction(@event.Action)
                    .SetSuffix(@event.Suffix)
                    .SetActionImage(@event.ActionImgUrl)
                )
            );
    }

    public static EventStructure ToNoticeEvent(this FriendRecallEvent @event) {
        return new EventStructure()
            .SetType(EventType.Notice)
            .SetNotice(new NoticeEvent()
                .SetType(NoticeType.PrivatePoke)
                .SetTime((ulong)new DateTimeOffset(@event.EventTime).ToUnixTimeSeconds())
                .SetNoticeId(Guid.NewGuid().ToString())
                .SetPrivateRecall(new PrivateRecallNotice()
                    .SetOperatorUin(@event.FriendUin)
                    .SetMessageId(MessageIdUtility.BuildMessageId(@event.Time, @event.Sequence))
                // .SetTipText(@event.) // TODO: Lagrange NotSupport
                )
            );
    }

    public static EventStructure ToNoticeEvent(this GroupPokeEvent @event) {
        return new EventStructure()
            .SetType(EventType.Notice)
            .SetNotice(new NoticeEvent()
                .SetType(NoticeType.GroupPoke)
                .SetTime((ulong)new DateTimeOffset(@event.EventTime).ToUnixTimeSeconds())
                .SetNoticeId(Guid.NewGuid().ToString())
                .SetGroupPoke(new GroupPokeNotice()
                    .SetGroupId(@event.GroupUin)
                    .SetOperatorUin(@event.OperatorUin)
                    .SetTargetUin(@event.TargetUin)
                    .SetAction(@event.Action)
                    .SetSuffix(@event.Suffix)
                    .SetActionImage(@event.ActionImgUrl)
                )
            );
    }

    public static EventStructure ToNoticeEvent(this GroupRecallEvent @event) {
        return new EventStructure()
            .SetType(EventType.Notice)
            .SetNotice(new NoticeEvent()
                .SetType(NoticeType.GroupRecall)
                .SetTime((ulong)new DateTimeOffset(@event.EventTime).ToUnixTimeSeconds())
                .SetNoticeId(Guid.NewGuid().ToString())
                .SetGroupRecall(new GroupRecallNotice()
                    .SetGroupId(@event.GroupUin)
                    .SetOperatorUin(@event.OperatorUin)
                    .SetTargetUin(@event.AuthorUin)
                    .SetMessageId(MessageIdUtility.BuildMessageId(@event.Time, @event.Sequence))
                // .SetTipText(@event.) // TODO: Lagrange NotSupport
                )
            );
    }

    public static EventStructure ToNoticeEvent(this GroupEssenceEvent @event) {
        return new EventStructure()
            .SetType(EventType.Notice)
            .SetNotice(new NoticeEvent()
                .SetType(NoticeType.GroupEssenceChanged)
                .SetTime((ulong)new DateTimeOffset(@event.EventTime).ToUnixTimeSeconds())
                .SetNoticeId(Guid.NewGuid().ToString())
                .SetGroupEssenceChanged(new GroupEssenceMessageNotice()
                    .SetGroupId(@event.GroupUin)
                    .SetOperatorUin(@event.OperatorUin)
                    .SetTargetUin(@event.FromUin)
                    .SetMessageId(MessageIdUtility.BuildMessageId(0, @event.Sequence)) // TODO: Time
                    .SetIsSet(@event.IsSet)
                )
            );
    }

    public static EventStructure ToNoticeEvent(this GroupMemberIncreaseEvent @event) {
        return new EventStructure()
            .SetType(EventType.Notice)
            .SetNotice(new NoticeEvent()
                .SetType(NoticeType.GroupMemberIncrease)
                .SetTime((ulong)new DateTimeOffset(@event.EventTime).ToUnixTimeSeconds())
                .SetNoticeId(Guid.NewGuid().ToString())
                .SetGroupMemberIncrease(new GroupMemberIncreasedNotice()
                    .SetGroupId(@event.GroupUin)
                    // .SetOperatorUin() // TODO: Lagrange NotSupport
                    // .SetInvitorUin() // TODO: Kritor NotSupport
                    .SetTargetUin(@event.MemberUin)
                    .SetType(@event.Type switch {
                        GroupMemberIncreaseEvent.EventType.Approve => GroupMemberIncreasedType.Approve,
                        GroupMemberIncreaseEvent.EventType.Invite => GroupMemberIncreasedType.Invite,
                        _ => throw new Exception($"GroupMemberIncreaseEvent Unknown Type: {@event.Type}")
                    })
                )
            );
    }

    public static EventStructure ToNoticeEvent(this GroupMemberDecreaseEvent @event) {
        GroupMemberDecreasedNotice notice = new GroupMemberDecreasedNotice()
            .SetGroupId(@event.GroupUin)
            .SetTargetUin(@event.MemberUin)
            .SetType(@event.Type switch {
                GroupMemberDecreaseEvent.EventType.KickMe => GroupMemberDecreasedType.KickMe,
                GroupMemberDecreaseEvent.EventType.Leave => GroupMemberDecreasedType.Leave,
                GroupMemberDecreaseEvent.EventType.Kick => GroupMemberDecreasedType.Kick,
                _ => throw new Exception($"GroupMemberDecreaseEvent Unknown Type: {@event.Type}")
            });
        if (@event.OperatorUin.HasValue) notice.SetOperatorUin(@event.OperatorUin.Value);

        return new EventStructure()
            .SetType(EventType.Notice)
            .SetNotice(new NoticeEvent()
                .SetType(NoticeType.GroupMemberDecrease)
                .SetTime((ulong)new DateTimeOffset(@event.EventTime).ToUnixTimeSeconds())
                .SetNoticeId(Guid.NewGuid().ToString())
                .SetGroupMemberDecrease(notice)
            );
    }

    public static EventStructure ToNoticeEvent(this GroupAdminChangedEvent @event) {
        return new EventStructure()
            .SetType(EventType.Notice)
            .SetNotice(new NoticeEvent()
                .SetType(NoticeType.GroupAdminChanged)
                .SetTime((ulong)new DateTimeOffset(@event.EventTime).ToUnixTimeSeconds())
                .SetNoticeId(Guid.NewGuid().ToString())
                .SetGroupAdminChanged(new GroupAdminChangedNotice()
                    .SetGroupId(@event.GroupUin)
                    .SetTargetUin(@event.AdminUin)
                    .SetIsAdmin(@event.IsPromote)
                )
            );
    }

    public static EventStructure ToNoticeEvent(this GroupMemberMuteEvent @event) {
        uint operatorUin = @event.OperatorUin ?? throw new Exception("FriendMessageEvent cannot retrieve OperatorUin");

        return new EventStructure()
            .SetType(EventType.Notice)
            .SetNotice(new NoticeEvent()
                .SetType(NoticeType.GroupMemberBan)
                .SetTime((ulong)new DateTimeOffset(@event.EventTime).ToUnixTimeSeconds())
                .SetNoticeId(Guid.NewGuid().ToString())
                .SetGroupMemberBan(new GroupMemberBanNotice()
                    .SetGroupId(@event.GroupUin)
                    .SetOperatorUin(operatorUin)
                    .SetTargetUin(@event.TargetUin)
                    .SetDuration((int)@event.Duration)
                    .SetType(@event.Duration != 0 ? GroupMemberBanType.Ban : GroupMemberBanType.LiftBan)
                )
            );
    }

    public static EventStructure ToNoticeEvent(this GroupMuteEvent @event) {
        uint operatorUin = @event.OperatorUin ?? throw new Exception("FriendMessageEvent cannot retrieve OperatorUin");

        return new EventStructure()
            .SetType(EventType.Notice)
            .SetNotice(new NoticeEvent()
                .SetType(NoticeType.GroupWholeBan)
                .SetTime((ulong)new DateTimeOffset(@event.EventTime).ToUnixTimeSeconds())
                .SetNoticeId(Guid.NewGuid().ToString())
                .SetGroupWholeBan(new GroupWholeBanNotice()
                    .SetGroupId(@event.GroupUin)
                    .SetOperatorUin(operatorUin)
                    .SetIsBan(@event.IsMuted)
                )
            );
    }

    public static EventStructure ToNoticeEvent(this GroupReactionEvent @event) {
        return new EventStructure()
            .SetType(EventType.Notice)
            .SetNotice(new NoticeEvent()
                .SetType(NoticeType.GroupReactMessageWithEmoji)
                .SetTime((ulong)new DateTimeOffset(@event.EventTime).ToUnixTimeSeconds())
                .SetNoticeId(Guid.NewGuid().ToString())
                .SetGroupReactMessageWithEmoji(new GroupReactMessageWithEmojiNotice()
                    .SetGroupId(@event.TargetGroupUin)
                    .SetMessageId(MessageIdUtility.BuildMessageId(0, @event.TargetSequence)) // TODO: Time
                    .SetFaceId(uint.Parse(@event.Code))
                    .SetIsSet(@event.IsAdd)
                )
            );
    }
}