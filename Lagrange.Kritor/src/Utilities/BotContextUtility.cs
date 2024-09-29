using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Lagrange.Core;
using Lagrange.Core.Common.Interface.Api;
using Lagrange.Core.Message;

namespace Lagrange.Kritor.Utilities;

public static class BotContextUtility {
    public static async Task<MessageChain> GetMessageByMessageIdAsync(this BotContext bot, string id, CancellationToken token) {
        uint sequence = MessageIdUtility.GetSequence(id);
        uint uin = MessageIdUtility.GetUin(id);

        List<MessageChain>? chains;
        if (MessageIdUtility.IsGroup(id)) {
            chains = await bot.GetGroupMessage(uin, sequence, sequence);
        } else if (MessageIdUtility.IsGroup(id)) {
            chains = await bot.GetC2cMessage(uin, sequence, sequence);
        } else throw new NotSupportedException($"Not supported message id({id})");

        if (chains == null || chains.Count != 1) {
            throw new Exception($"Get message chain by message id({id}) failed");
        }

        return chains[0];
    }
}