using System.Reflection;
using System.Threading.Tasks;
using Grpc.Core;
using Lagrange.Core;
using Microsoft.Extensions.Logging;
using GrpcInterceptor = Grpc.Core.Interceptors.Interceptor;

namespace Lagrange.Kritor.Interceptors;

public class KritorVersionInterceptor(ILogger<KritorVersionInterceptor> logger, BotContext bot) : GrpcInterceptor {
    private readonly ILogger<KritorVersionInterceptor> _logger = logger;

    private readonly string _uin = bot.BotUin.ToString();

    private readonly string _version = Assembly.GetAssembly(typeof(Program))?
            .GetCustomAttribute<AssemblyInformationalVersionAttribute>()?
            .InformationalVersion ?? "Unknown";

    public override Task<TResponse> UnaryServerHandler<TRequest, TResponse>(TRequest request, ServerCallContext context, UnaryServerMethod<TRequest, TResponse> continuation) {
        context.ResponseTrailers.Add("kritor-self-uin", _uin);
        context.ResponseTrailers.Add("kritor-self-uid", "");
        context.ResponseTrailers.Add("kritor-self-version", $"Lagrange.Kritor {_version}");
        return base.UnaryServerHandler(request, context, continuation);
    }
}