using System;
using System.Collections.Generic;
using System.Linq;
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
            Scene.StrangerFromGroup => throw new NotSupportedException(
                $"Not supported Scene({Scene.StrangerFromGroup})"
            ),
            Scene.Nearby => throw new NotSupportedException($"Not supported Scene({Scene.Nearby})"),
            Scene.Stranger => throw new NotSupportedException($"Not supported Scene({Scene.Stranger})"),
            _ => throw new NotSupportedException($"Not supported Scene({request.Contact.Scene})"),
        };

        MessageResult result = await _bot.SendMessage(chain);
        if (result.Result != 0 || result.Sequence == null) {
            throw new Exception($"(Code {result.Result}) Send message failed");
        }

        return new SendMessageResponse {
            MessageId = MessageIdUtility.BuildMessageId(chain, result),
            MessageTime = result.Timestamp
        };
    }

    public override async Task<SendMessageByResIdResponse> SendMessageByResId(SendMessageByResIdRequest request, ServerCallContext context) {
        (int code, List<MessageChain>? chains) = await _bot.GetMessagesByResId(request.ResId);

        if (code != 0) throw new Exception($"(Code {code}) Get messages by res id failed");

        if (chains == null) throw new Exception($"Get messages by res id result is null");

        uint uin = uint.Parse(request.Contact.Peer);

        MessageChain chain = request.Contact.Scene switch {
            Scene.Unspecified => throw new NotSupportedException($"Not supported Scene({Scene.Unspecified})"),
            Scene.Group => MessageBuilder.Group(uin).MultiMsg([.. chains]).Build(),
            Scene.Friend => MessageBuilder.Friend(uin).MultiMsg([.. chains]).Build(),
            Scene.Guild => throw new NotSupportedException($"Not supported Scene({Scene.Guild})"),
            Scene.StrangerFromGroup => throw new NotSupportedException(
                $"Not supported Scene({Scene.StrangerFromGroup})"
            ),
            Scene.Nearby => throw new NotSupportedException($"Not supported Scene({Scene.Nearby})"),
            Scene.Stranger => throw new NotSupportedException($"Not supported Scene({Scene.Stranger})"),
            _ => throw new NotSupportedException($"Not supported Scene({request.Contact.Scene})")
        };

        MessageResult result = await _bot.SendMessage(chain);
        if (result.Result != 0 || result.Sequence == null) {
            throw new Exception($"(Code {result.Result}) Send message failed");
        }

        return new SendMessageByResIdResponse {
            MessageId = MessageIdUtility.BuildMessageId(chain, result),
            MessageTime = result.Timestamp
        };
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
            Scene.StrangerFromGroup => throw new NotSupportedException(
                $"Not supported Scene({Scene.StrangerFromGroup})"
            ),
            Scene.Nearby => throw new NotSupportedException($"Not supported Scene({Scene.Nearby})"),
            Scene.Stranger => throw new NotSupportedException($"Not supported Scene({Scene.Stranger})"),
            _ => throw new NotSupportedException($"Not supported Scene({request.Contact.Scene})")
        };

        if (!isSuccess) throw new Exception($"Recall group/friend message failed");

        return new RecallMessageResponse { };
    }

    public override async Task<ReactMessageWithEmojiResponse> ReactMessageWithEmoji(ReactMessageWithEmojiRequest request, ServerCallContext context) {
        if (request.Contact.Scene != Scene.Group) {
            throw new NotSupportedException($"Not supported Scene({request.Contact.Scene})");
        }

        bool isSuccess = await _bot.GroupSetMessageReaction(
            MessageIdUtility.GetUin(request.MessageId),
            MessageIdUtility.GetSequence(request.MessageId),
            $"{request.FaceId}",
            request.IsSet
        );

        if (!isSuccess) throw new Exception($"Group set message reaction failed");

        return new ReactMessageWithEmojiResponse { };
    }

    public override async Task<GetMessageResponse> GetMessage(GetMessageRequest request, ServerCallContext context) {
        MessageChain chain = await _bot.GetMessageByMessageIdAsync(request.MessageId, context.CancellationToken);
        return new GetMessageResponse { Message = chain.ToPushMessageBody() };
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
            Scene.StrangerFromGroup => throw new NotSupportedException(
                $"Not supported Scene({Scene.StrangerFromGroup})"
            ),
            Scene.Nearby => throw new NotSupportedException($"Not supported Scene({Scene.Nearby})"),
            Scene.Stranger => throw new NotSupportedException($"Not supported Scene({Scene.Stranger})"),
            _ => throw new NotSupportedException($"Not supported Scene({request.Contact.Scene})")
        };

        if (chains == null || chains.Count == 0) throw new Exception($"Get group/c2c message failed");

        return new GetMessageBySeqResponse { Message = chains[0].ToPushMessageBody() };
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
            Scene.StrangerFromGroup => throw new NotSupportedException(
                $"Not supported Scene({Scene.StrangerFromGroup})"
            ),
            Scene.Nearby => throw new NotSupportedException($"Not supported Scene({Scene.Nearby})"),
            Scene.Stranger => throw new NotSupportedException($"Not supported Scene({Scene.Stranger})"),
            _ => throw new NotSupportedException($"Not supported Scene({request.Contact.Scene})")
        } ?? throw new Exception($"Get group/c2c message failed");

        return new GetHistoryMessageResponse { Messages = { chains.Select(MessageConverter.ToPushMessageBody) } };
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
            _ => throw new NotSupportedException($"Not supported Scene({request.Contact.Scene})")
        } ?? throw new Exception($"Get group/c2c message failed");

        return new GetHistoryMessageBySeqResponse { Messages = { chains.Select(MessageConverter.ToPushMessageBody) } };
    }

    // WAITIMPL: Lagrange.Core
    public override Task<UploadForwardMessageResponse> UploadForwardMessage(UploadForwardMessageRequest request, ServerCallContext context) {
        return base.UploadForwardMessage(request, context);
    }

    public override async Task<DownloadForwardMessageResponse> DownloadForwardMessage(DownloadForwardMessageRequest request, ServerCallContext context) {
        (int code, List<MessageChain>? chains) = await _bot.GetMessagesByResId(request.ResId);

        if (code != 0) throw new Exception($"(Code {code}) Get mmessages by res id failed");

        if (chains == null) return new DownloadForwardMessageResponse { Messages = { } };

        return new DownloadForwardMessageResponse { Messages = { chains.Select(MessageConverter.ToPushMessageBody) } };
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