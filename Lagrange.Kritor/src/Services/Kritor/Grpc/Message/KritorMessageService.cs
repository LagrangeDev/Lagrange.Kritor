using System;
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

    // TODO I'm tired. I'll do it next time.
}