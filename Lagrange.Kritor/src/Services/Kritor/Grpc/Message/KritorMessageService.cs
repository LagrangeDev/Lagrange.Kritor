using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Grpc.Core;
using Kritor.Common;
using Kritor.Message;
using Lagrange.Core;
using Lagrange.Core.Common.Interface.Api;
using Lagrange.Core.Message;
using Lagrange.Kritor.Converters;
using Lagrange.Kritor.Utilities;
using static Kritor.Message.MessageService;

namespace Lagrange.Kritor.Services.Kritor.Grpc.Message;

public class KritorMessageService(BotContext bot) : MessageServiceBase {
    private readonly BotContext _bot = bot;

    public override async Task<SendMessageResponse> SendMessage(SendMessageRequest request, ServerCallContext context) {
        uint uin = uint.Parse(request.Contact.Peer);

        MessageChain chain = request.Contact.Scene switch {
            Scene.Unspecified => throw new NotSupportedException($"Not supported Scene({Scene.Unspecified})"),
            Scene.Group => await request.Elements.ToGroupChainAsync(_bot, uin, context.CancellationToken),
            Scene.Friend => await request.Elements.ToFriendChainAsync(_bot, uin, context.CancellationToken),
            Scene.Guild => throw new NotSupportedException($"Not supported Scene({Scene.Guild})"),
            Scene.StrangerFromGroup => throw new NotSupportedException($"Not supported Scene({Scene.StrangerFromGroup})"),
            Scene.Nearby => throw new NotSupportedException($"Not supported Scene({Scene.Nearby})"),
            Scene.Stranger => throw new NotSupportedException($"Not supported Scene({Scene.Stranger})"),
            Scene unknown => throw new NotSupportedException($"Not supported Scene({unknown})"),
        };

        MessageResult result = await _bot.SendMessage(chain);
        if (result.Result != 0 || result.Sequence == null) {
            throw new Exception($"({result.Result}) Send message failed");
        }

        return new SendMessageResponse {
            MessageId = MessageIdUtility.BuildGroupMessageId(uin, (ulong)result.Sequence),
            MessageTime = result.Timestamp
        };
    }

    // WAITMERGE: https://github.com/LagrangeDev/Lagrange.Core/pull/616
    public override Task<SendMessageByResIdResponse> SendMessageByResId(SendMessageByResIdRequest request, ServerCallContext context) {
        return base.SendMessageByResId(request, context);
    }

    public override Task<SetMessageReadResponse> SetMessageReaded(SetMessageReadRequest request, ServerCallContext context) {
        return base.SetMessageReaded(request, context);
    }

    public override async Task<RecallMessageResponse> RecallMessage(RecallMessageRequest request, ServerCallContext context) {
        uint uin = MessageIdUtility.GetUin(request.MessageId);
        uint sequence = MessageIdUtility.GetSequence(request.MessageId);

        bool isSuccess = request.Contact.Scene switch {
            Scene.Unspecified => throw new NotSupportedException($"Not supported Scene({Scene.Unspecified})"),
            Scene.Group => await _bot.RecallGroupMessage(uin, sequence),
            Scene.Friend => await _bot.RecallFriendMessage(
                await _bot.GetMessageByMessageIdAsync(request.MessageId, context.CancellationToken)
            ),
            Scene.Guild => throw new NotSupportedException($"Not supported Scene({Scene.Guild})"),
            Scene.StrangerFromGroup => throw new NotSupportedException($"Not supported Scene({Scene.StrangerFromGroup})"),
            Scene.Nearby => throw new NotSupportedException($"Not supported Scene({Scene.Nearby})"),
            Scene.Stranger => throw new NotSupportedException($"Not supported Scene({Scene.Stranger})"),
            Scene unknown => throw new NotSupportedException($"Not supported Scene({unknown})")
        };

        if (!isSuccess) throw new Exception($"Recall message");

        return new RecallMessageResponse { };
    }

    public override async Task<ReactMessageWithEmojiResponse> ReactMessageWithEmoji(ReactMessageWithEmojiRequest request, ServerCallContext context) {
        if (request.Contact.Scene != Scene.Group) {
            throw new NotSupportedException($"Not supported Scene({request.Contact.Scene})");
        }

        // WAITIMPL: Lagrange.Core
        if (!request.IsSet) {
            throw new NotSupportedException($"Not supported recall react");
        }

        bool isSuccess = await _bot.GroupSetMessageReaction(
            MessageIdUtility.GetUin(request.MessageId),
            MessageIdUtility.GetSequence(request.MessageId),
            $"{request.FaceId}"
        );

        if (!isSuccess) throw new Exception($"React message with emoji failed");

        return new ReactMessageWithEmojiResponse { };
    }

    public override async Task<GetMessageResponse> GetMessage(GetMessageRequest request, ServerCallContext context) {
        switch (request.Contact.Scene) {
            case Scene.Unspecified: { throw new NotSupportedException($"Not supported Scene({Scene.Unspecified})"); }
            case Scene.Group: { break; }
            case Scene.Friend: { break; }
            case Scene.Guild: { throw new NotSupportedException($"Not supported Scene({Scene.Guild})"); }
            case Scene.StrangerFromGroup: { throw new NotSupportedException($"Not supported Scene({Scene.StrangerFromGroup})"); }
            case Scene.Nearby: { throw new NotSupportedException($"Not supported Scene({Scene.Nearby})"); }
            case Scene.Stranger: { throw new NotSupportedException($"Not supported Scene({Scene.Stranger})"); }
            case Scene unknown: { throw new NotSupportedException($"Not supported Scene({unknown})"); }
        }

        MessageChain chain = await _bot.GetMessageByMessageIdAsync(request.MessageId, context.CancellationToken);

        return new GetMessageResponse {
            Message = new PushMessageBody {
                Time = (ulong)new DateTimeOffset(chain.Time).ToUnixTimeSeconds(),
                MessageId = MessageIdUtility.BuildMessageId(chain),
                MessageSeq = chain.Sequence,
                Private = new PrivateSender {
                    Uin = chain.FriendUin,
                    Nick = chain.FriendInfo?.Nickname
                        ?? throw new Exception("`FriendMessageEvent.Chain.FriendInfo.Nickname` is null")
                },
                Elements = { chain.ToElements() }
            }
        };
    }

    public override async Task<GetMessageBySeqResponse> GetMessageBySeq(GetMessageBySeqRequest request, ServerCallContext context) {
        uint uin = uint.Parse(request.Contact.Peer);

        List<MessageChain>? chains = request.Contact.Scene switch {
            Scene.Unspecified => throw new NotSupportedException($"Not supported Scene({Scene.Unspecified})"),
            Scene.Group => await _bot.GetGroupMessage(
                uin,
                (uint)request.MessageSeq,
                (uint)request.MessageSeq
            ),
            Scene.Friend => await _bot.GetC2cMessage(
                uin,
                (uint)request.MessageSeq,
                (uint)request.MessageSeq
            ),
            Scene.Guild => throw new NotSupportedException($"Not supported Scene({Scene.Guild})"),
            Scene.StrangerFromGroup => throw new NotSupportedException($"Not supported Scene({Scene.StrangerFromGroup})"),
            Scene.Nearby => throw new NotSupportedException($"Not supported Scene({Scene.Nearby})"),
            Scene.Stranger => throw new NotSupportedException($"Not supported Scene({Scene.Stranger})"),
            Scene unknown => throw new NotSupportedException($"Not supported Scene({unknown})")
        };

        if (chains == null || chains.Count == 0) throw new Exception($"Get message by seq failed");

        return new GetMessageBySeqResponse {
            Message = new PushMessageBody {
                Time = (ulong)new DateTimeOffset(chains[0].Time).ToUnixTimeSeconds(),
                MessageId = MessageIdUtility.BuildMessageId(chains[0]),
                MessageSeq = chains[0].Sequence,
                Private = new PrivateSender {
                    Uin = chains[0].FriendUin,
                    Nick = chains[0].FriendInfo?.Nickname
                        ?? throw new Exception("`FriendMessageEvent.Chain.FriendInfo.Nickname` is null")
                },
                Elements = { chains[0].ToElements() }
            }
        };
    }

    public override async Task<GetHistoryMessageResponse> GetHistoryMessage(GetHistoryMessageRequest request, ServerCallContext context) {
        uint uin = uint.Parse(request.Contact.Peer);

        List<MessageChain> chains = request.Contact.Scene switch {
            Scene.Unspecified => throw new NotSupportedException($"Not supported Scene({Scene.Unspecified})"),
            Scene.Group => await _bot.GetGroupMessage(
                uin,
                MessageIdUtility.GetSequence(request.StartMessageId) - request.Count + 1,
                MessageIdUtility.GetSequence(request.StartMessageId)
            ),
            Scene.Friend => await _bot.GetC2cMessage(
                uin,
                MessageIdUtility.GetSequence(request.StartMessageId) - request.Count + 1,
                MessageIdUtility.GetSequence(request.StartMessageId)
            ),
            Scene.Guild => throw new NotSupportedException($"Not supported Scene({Scene.Guild})"),
            Scene.StrangerFromGroup => throw new NotSupportedException($"Not supported Scene({Scene.StrangerFromGroup})"),
            Scene.Nearby => throw new NotSupportedException($"Not supported Scene({Scene.Nearby})"),
            Scene.Stranger => throw new NotSupportedException($"Not supported Scene({Scene.Stranger})"),
            Scene unknown => throw new NotSupportedException($"Not supported Scene({unknown})")
        } ?? throw new Exception($"Get history message failed");

        IEnumerable<PushMessageBody> messages = chains.Select(chain => new PushMessageBody {
            Time = (ulong)new DateTimeOffset(chain.Time).ToUnixTimeSeconds(),
            MessageId = MessageIdUtility.BuildMessageId(chain),
            MessageSeq = chain.Sequence,
            Private = new PrivateSender {
                Uin = chain.FriendUin,
                Nick = chain.FriendInfo?.Nickname
                    ?? throw new Exception("`FriendMessageEvent.Chain.FriendInfo.Nickname` is null")
            },
            Elements = { chain.ToElements() }
        });

        return new GetHistoryMessageResponse {
            Messages = { messages }
        };
    }

    public override async Task<GetHistoryMessageBySeqResponse> GetHistoryMessageBySeq(GetHistoryMessageBySeqRequest request, ServerCallContext context) {
        uint uin = uint.Parse(request.Contact.Peer);

        List<MessageChain> chains = request.Contact.Scene switch {
            Scene.Unspecified => throw new NotSupportedException($"Not supported Scene({Scene.Unspecified})"),
            Scene.Group => await _bot.GetGroupMessage(
                uin,
                (uint)(request.StartMessageSeq - request.Count + 1),
                (uint)request.StartMessageSeq
            ),
            Scene.Friend => await _bot.GetC2cMessage(
                uin,
                (uint)(request.StartMessageSeq - request.Count + 1),
                (uint)request.StartMessageSeq
            ),
            Scene.Guild => throw new NotSupportedException($"Not supported Scene({Scene.Guild})"),
            Scene.StrangerFromGroup => throw new NotSupportedException($"Not supported Scene({Scene.StrangerFromGroup})"),
            Scene.Nearby => throw new NotSupportedException($"Not supported Scene({Scene.Nearby})"),
            Scene.Stranger => throw new NotSupportedException($"Not supported Scene({Scene.Stranger})"),
            Scene unknown => throw new NotSupportedException($"Not supported Scene({unknown})")
        } ?? throw new Exception($"Get history message by seq failed");

        IEnumerable<PushMessageBody> messages = chains.Select(chain => new PushMessageBody {
            Time = (ulong)new DateTimeOffset(chain.Time).ToUnixTimeSeconds(),
            MessageId = MessageIdUtility.BuildMessageId(chain),
            MessageSeq = chain.Sequence,
            Private = new PrivateSender {
                Uin = chain.FriendUin,
                Nick = chain.FriendInfo?.Nickname
                    ?? throw new Exception("`FriendMessageEvent.Chain.FriendInfo.Nickname` is null")
            },
            Elements = { chain.ToElements() }
        });

        return new GetHistoryMessageBySeqResponse {
            Messages = { messages }
        };
    }

    // WAITIMPL: Lagrange.Core
    public override Task<UploadForwardMessageResponse> UploadForwardMessage(UploadForwardMessageRequest request, ServerCallContext context) {
        return base.UploadForwardMessage(request, context);
    }

    // WAITMERGE: https://github.com/LagrangeDev/Lagrange.Core/pull/616
    public override Task<DownloadForwardMessageResponse> DownloadForwardMessage(DownloadForwardMessageRequest request, ServerCallContext context) {
        return base.DownloadForwardMessage(request, context);
    }

    // TODO: Need to look into it. (；′⌒`)
    public override Task<GetEssenceMessageListResponse> GetEssenceMessageList(GetEssenceMessageListRequest request, ServerCallContext context) {
        return base.GetEssenceMessageList(request, context);
    }

    // TODO: Need to look into it. (；′⌒`)
    public override Task<SetEssenceMessageResponse> SetEssenceMessage(SetEssenceMessageRequest request, ServerCallContext context) {
        return base.SetEssenceMessage(request, context);
    }

    // TODO: Need to look into it. (；′⌒`)
    public override Task<DeleteEssenceMessageResponse> DeleteEssenceMessage(DeleteEssenceMessageRequest request, ServerCallContext context) {
        return base.DeleteEssenceMessage(request, context);
    }
}