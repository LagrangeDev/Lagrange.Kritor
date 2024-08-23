using System.Threading.Tasks;
using Grpc.Core;
using Kritor.Core;
using Lagrange.Core;
using static Kritor.Core.CoreService;

namespace Lagrange.Kritor.Service.Kritor.Grpc.Core;

public class KritorCoreService(BotContext bot) : CoreServiceBase {
    public override Task<GetVersionResponse> GetVersion(GetVersionRequest request, ServerCallContext context) {
        return Task.FromResult(GetVersionResponse.Create("0.0.0-alpha", "Lagrange.Kritor"));
    }

    // TODO: DownloadFile
    // Waiting for the result of the PR

    public override Task<GetCurrentAccountResponse> GetCurrentAccount(GetCurrentAccountRequest request, ServerCallContext context) {
        return Task.FromResult(GetCurrentAccountResponse.Create(bot.BotUin.ToString(), bot.BotUin, bot.BotName ?? ""));
    }
}