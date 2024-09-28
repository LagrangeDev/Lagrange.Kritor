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

    private async Task<SendMessageResponse> SendGroupMessage(SendMessageRequest request, CancellationToken token) {
        uint groupUin = uint.Parse(request.Contact.Peer);

        MessageResult result = await _bot.SendMessage(request.Elements.ToGroupChain(groupUin));
        if (result.Result != 0 || result.Sequence == null) {
            throw new Exception($"Send group message failed");
        }

        return new SendMessageResponse {
            MessageId = MessageIdUtility.BuildGroupMessageId(groupUin, (ulong)result.Sequence),
            MessageTime = result.Timestamp
        };
    }

    private async Task<SendMessageResponse> SendFriendMessage(SendMessageRequest request, CancellationToken token) {
        uint groupUin = uint.Parse(request.Contact.Peer);

        MessageResult result = await _bot.SendMessage(request.Elements.ToFriendChain(groupUin));
        if (result.Result != 0 || result.Sequence == null) {
            throw new Exception($"Send friend message failed");
        }

        return new SendMessageResponse {
            MessageId = MessageIdUtility.BuildGroupMessageId(groupUin, (ulong)result.Sequence),
            MessageTime = result.Timestamp
        };
    }

    public override Task<SendMessageResponse> SendMessage(SendMessageRequest request, ServerCallContext context) {
        return request.Contact.Scene switch {
            Scene.Unspecified => throw new NotSupportedException($"Not supported Scene({Scene.Unspecified})"),
            Scene.Group => SendGroupMessage(request, context.CancellationToken),
            Scene.Friend => SendFriendMessage(request, context.CancellationToken),
            Scene.Guild => throw new NotSupportedException($"Not supported Scene({Scene.Guild})"),
            Scene.StrangerFromGroup => throw new NotSupportedException($"Not supported Scene({Scene.StrangerFromGroup})"),
            Scene.Nearby => throw new NotSupportedException($"Not supported Scene({Scene.Nearby})"),
            Scene.Stranger => throw new NotSupportedException($"Not supported Scene({Scene.Stranger})"),
            Scene unknown => throw new NotSupportedException($"Not supported Scene({unknown})"),
        };
    }

    // TODO I'm tired. I'll do it next time.
}