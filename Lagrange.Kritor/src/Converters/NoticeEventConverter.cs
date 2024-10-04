using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Kritor.Event;
using Lagrange.Core;
using Lagrange.Core.Common.Interface.Api;
using Lagrange.Core.Event;
using Lagrange.Core.Event.EventArg;
using Lagrange.Core.Message;
using Lagrange.Kritor.Utilities;
using static Kritor.Event.GroupMemberBanNotice.Types;
using static Kritor.Event.GroupMemberDecreasedNotice.Types;
using static Kritor.Event.GroupMemberIncreasedNotice.Types;
using static Kritor.Event.NoticeEvent.Types;

namespace Lagrange.Kritor.Converters;

public static class NoticeEventConverter {
    public static async Task<EventStructure> ToNoticeEvent(this EventBase @event, BotContext bot, CancellationToken token) {
        return @event switch {
            FriendPokeEvent poke => new EventStructure {
                Type = EventType.Notice,
                Notice = new NoticeEvent {
                    Type = NoticeType.PrivatePoke,
                    Time = (ulong)new DateTimeOffset(poke.EventTime).ToUnixTimeSeconds(),
                    NoticeId = Guid.NewGuid().ToString(),
                    PrivatePoke = new PrivatePokeNotice {
                        OperatorUin = poke.OperatorUin,
                        Action = poke.Action,
                        Suffix = poke.Suffix,
                        ActionImage = poke.ActionImgUrl
                    }
                }
            },
            FriendRecallEvent recall => await recall.ToNoticeEvent(bot, token),
            GroupPokeEvent poke => new EventStructure {
                Type = EventType.Notice,
                Notice = new NoticeEvent {
                    Type = NoticeType.GroupPoke,
                    Time = (ulong)new DateTimeOffset(poke.EventTime).ToUnixTimeSeconds(),
                    NoticeId = Guid.NewGuid().ToString(),
                    GroupPoke = new GroupPokeNotice {
                        GroupId = poke.GroupUin,
                        OperatorUin = poke.OperatorUin,
                        TargetUin = poke.TargetUin,
                        Action = poke.Action,
                        Suffix = poke.Suffix,
                        ActionImage = poke.ActionImgUrl
                    }
                }
            },
            GroupRecallEvent recall => new EventStructure {
                Type = EventType.Notice,
                Notice = new NoticeEvent {
                    Type = NoticeType.GroupRecall,
                    Time = (ulong)new DateTimeOffset(recall.EventTime).ToUnixTimeSeconds(),
                    NoticeId = Guid.NewGuid().ToString(),
                    GroupRecall = new GroupRecallNotice {
                        GroupId = recall.GroupUin,
                        OperatorUin = recall.OperatorUin,
                        TargetUin = recall.AuthorUin,
                        MessageId = MessageIdUtility.BuildGroupMessageId(recall.GroupUin, recall.Sequence),
                        TipText = recall.Tip
                    }
                }
            },
            GroupEssenceEvent essence => new EventStructure {
                Type = EventType.Notice,
                Notice = new NoticeEvent {
                    Type = NoticeType.GroupEssenceChanged,
                    Time = (ulong)new DateTimeOffset(essence.EventTime).ToUnixTimeSeconds(),
                    NoticeId = Guid.NewGuid().ToString(),
                    GroupEssenceChanged = new GroupEssenceMessageNotice {
                        GroupId = essence.GroupUin,
                        OperatorUin = essence.OperatorUin,
                        TargetUin = essence.FromUin,
                        MessageId = MessageIdUtility.BuildGroupMessageId(essence.GroupUin, essence.Sequence),
                        IsSet = essence.IsSet
                    }
                }
            },
            GroupMemberIncreaseEvent increase => new EventStructure {
                Type = EventType.Notice,
                Notice = new NoticeEvent {
                    Type = NoticeType.GroupMemberIncrease,
                    Time = (ulong)new DateTimeOffset(increase.EventTime).ToUnixTimeSeconds(),
                    NoticeId = Guid.NewGuid().ToString(),
                    GroupMemberIncrease = new GroupMemberIncreasedNotice {
                        GroupId = increase.GroupUin,
                        // OperatorUin = increase.
                        TargetUin = increase.MemberUin,
                        Type = increase.Type switch {
                            GroupMemberIncreaseEvent.EventType.Approve => GroupMemberIncreasedType.Approve,
                            GroupMemberIncreaseEvent.EventType.Invite => GroupMemberIncreasedType.Invite,
                            _ => GroupMemberIncreasedType.Unspecified,
                        }
                    }
                }
            },
            GroupMemberDecreaseEvent decrease => new EventStructure {
                Type = EventType.Notice,
                Notice = new NoticeEvent {
                    Type = NoticeType.GroupMemberDecrease,
                    Time = (ulong)new DateTimeOffset(decrease.EventTime).ToUnixTimeSeconds(),
                    NoticeId = Guid.NewGuid().ToString(),
                    GroupMemberDecrease = new GroupMemberDecreasedNotice {
                        GroupId = decrease.GroupUin,
                        TargetUin = decrease.MemberUin,
                        OperatorUin = decrease.OperatorUin ?? 0,
                        Type = decrease.Type switch {
                            GroupMemberDecreaseEvent.EventType.KickMe => GroupMemberDecreasedType.KickMe,
                            GroupMemberDecreaseEvent.EventType.Leave => GroupMemberDecreasedType.Leave,
                            GroupMemberDecreaseEvent.EventType.Kick => GroupMemberDecreasedType.Kick,
                            _ => GroupMemberDecreasedType.Unspecified,
                        }
                    }
                }
            },
            GroupAdminChangedEvent changed => new EventStructure {
                Type = EventType.Notice,
                Notice = new NoticeEvent {
                    Type = NoticeType.GroupAdminChanged,
                    Time = (ulong)new DateTimeOffset(changed.EventTime).ToUnixTimeSeconds(),
                    NoticeId = Guid.NewGuid().ToString(),
                    GroupAdminChanged = new GroupAdminChangedNotice {
                        GroupId = changed.GroupUin,
                        TargetUin = changed.AdminUin,
                        IsAdmin = changed.IsPromote
                    }
                }
            },
            GroupMemberMuteEvent mute => new EventStructure {
                Type = EventType.Notice,
                Notice = new NoticeEvent {
                    Type = NoticeType.GroupMemberBan,
                    Time = (ulong)new DateTimeOffset(mute.EventTime).ToUnixTimeSeconds(),
                    NoticeId = Guid.NewGuid().ToString(),
                    GroupMemberBan = new GroupMemberBanNotice {
                        GroupId = mute.GroupUin,
                        OperatorUin = (ulong)(mute.OperatorUin ?? null!),
                        TargetUin = mute.TargetUin,
                        Duration = (int)mute.Duration,
                        Type = mute.Duration != 0 ? GroupMemberBanType.Ban : GroupMemberBanType.LiftBan,
                    }
                }
            },
            GroupMuteEvent mute => new EventStructure {
                Type = EventType.Notice,
                Notice = new NoticeEvent {
                    Type = NoticeType.GroupWholeBan,
                    Time = (ulong)new DateTimeOffset(mute.EventTime).ToUnixTimeSeconds(),
                    NoticeId = Guid.NewGuid().ToString(),
                    GroupWholeBan = new GroupWholeBanNotice {
                        GroupId = mute.GroupUin,
                        OperatorUin = (ulong)(mute.OperatorUin ?? null!),
                        IsBan = mute.IsMuted,
                    }
                }
            },
            GroupReactionEvent reaction => new EventStructure {
                Type = EventType.Notice,
                Notice = new NoticeEvent {
                    Type = NoticeType.GroupReactMessageWithEmoji,
                    Time = (ulong)new DateTimeOffset(reaction.EventTime).ToUnixTimeSeconds(),
                    NoticeId = Guid.NewGuid().ToString(),
                    GroupReactMessageWithEmoji = new GroupReactMessageWithEmojiNotice {
                        GroupId = reaction.TargetGroupUin,
                        MessageId = MessageIdUtility.BuildGroupMessageId(reaction.TargetGroupUin, reaction.TargetSequence),
                        FaceId = uint.Parse(reaction.Code),
                        IsSet = reaction.IsAdd
                    }
                }
            },
            _ => throw new NotSupportedException($"Not supported Event({@event})"),
        };
    }

    public static async Task<EventStructure> ToNoticeEvent(this FriendRecallEvent @event, BotContext bot, CancellationToken token) {
        List<MessageChain>? chain = await bot.GetRoamMessage(@event.FriendUin, @event.Time, 1);
        if (chain == null || chain.Count == 0) throw new Exception($"get roam message failed");

        return new EventStructure {
            Type = EventType.Notice,
            Notice = new NoticeEvent {
                Type = NoticeType.PrivateRecall,
                Time = (ulong)new DateTimeOffset(@event.EventTime).ToUnixTimeSeconds(),
                NoticeId = Guid.NewGuid().ToString(),
                PrivateRecall = new PrivateRecallNotice {
                    OperatorUin = @event.FriendUin,
                    MessageId = MessageIdUtility.BuildGroupMessageId(@event.FriendUin, chain[0].Sequence),
                    TipText = @event.Tip
                }
            }
        };
    }
}