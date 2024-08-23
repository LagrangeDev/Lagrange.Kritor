using System.Reflection;
using System.Threading.Tasks;
using Grpc.Core;
using Kritor.Core;
using Lagrange.Core;
using static Kritor.Core.CoreService;

namespace Lagrange.Kritor.Service.Kritor.Grpc.Core;

public class KritorCoreService(BotContext bot) : CoreServiceBase {
    private readonly string _version = Assembly.GetAssembly(typeof(Program))?
        .GetCustomAttribute<AssemblyInformationalVersionAttribute>()?
        .InformationalVersion ?? "Unknown";

    public override Task<GetVersionResponse> GetVersion(GetVersionRequest request, ServerCallContext context) {
        return Task.FromResult(new GetVersionResponse().SetAppName("Lagrange.Kritor").SetVersion(_version));
    }

    // TODO: DownloadFile
    // Waiting for the result of the PR

    public override Task<GetCurrentAccountResponse> GetCurrentAccount(GetCurrentAccountRequest request, ServerCallContext context) {
        return Task.FromResult(new GetCurrentAccountResponse()
            .SetAccountUin(bot.BotUin.ToString())
            .SetAccountName(bot.BotName ?? "")
        );
    }
}