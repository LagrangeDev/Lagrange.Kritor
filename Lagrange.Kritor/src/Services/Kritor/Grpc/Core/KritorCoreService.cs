using System;
using System.Reflection;
using System.Threading.Tasks;
using Grpc.Core;
using Kritor.Core;
using Lagrange.Core;
using static Kritor.Core.CoreService;

namespace Lagrange.Kritor.Services.Kritor.Grpc.Core;

public class KritorCoreService(BotContext bot) : CoreServiceBase {
    private readonly string _version = Assembly.GetAssembly(typeof(Program))?
        .GetCustomAttribute<AssemblyInformationalVersionAttribute>()?
        .InformationalVersion ?? "Unknown";

    public override Task<GetVersionResponse> GetVersion(GetVersionRequest request, ServerCallContext context) {
        return Task.FromResult(new GetVersionResponse {
            AppName = "Lagrange.Kritor",
            Version = _version
        });
    }

    // 
    public override Task<DownloadFileResponse> DownloadFile(DownloadFileRequest request, ServerCallContext context) {
        return base.DownloadFile(request, context);
    }

    public override Task<GetCurrentAccountResponse> GetCurrentAccount(GetCurrentAccountRequest request, ServerCallContext context) {
        return Task.FromResult(new GetCurrentAccountResponse {
            AccountUin = bot.BotUin,
            AccountName = bot.BotName ?? throw new Exception("`bot.BotName` is null")
        });
    }

    // WONTSUPPORTED
    public override Task<SwitchAccountResponse> SwitchAccount(SwitchAccountRequest request, ServerCallContext context) {
        return base.SwitchAccount(request, context);
    }
}