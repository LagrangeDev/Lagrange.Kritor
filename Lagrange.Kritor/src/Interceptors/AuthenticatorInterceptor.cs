using System.Linq;
using System.Threading.Tasks;
using Google.Protobuf.Reflection;
using Grpc.Core;
using Grpc.Core.Interceptors;
using Kritor.Authentication;
using Lagrange.Kritor.Authenticates;
using Microsoft.Extensions.Logging;

namespace Lagrange.Kritor.Interceptors;

public class AuthenticatorInterceptor(ILogger<AuthenticatorInterceptor> logger, Authenticator authenticator) : Interceptor {
    private readonly ILogger<AuthenticatorInterceptor> _logger = logger;

    private readonly Authenticator _authenticator = authenticator;

    private readonly string[] _sikp = [
        ..AllMethodFullPath(AuthenticationService.Descriptor)
    ];

    public override Task<TResponse> UnaryServerHandler<TRequest, TResponse>(TRequest request, ServerCallContext context, UnaryServerMethod<TRequest, TResponse> continuation) {
        if (_sikp.Contains(context.Method)) return base.UnaryServerHandler(request, context, continuation);

        if (!_authenticator.IsEnabled) return base.UnaryServerHandler(request, context, continuation);

        Metadata.Entry? ticketEntry = context.RequestHeaders.Get("ticket");

        if (ticketEntry == null) {
            _logger.LogAuthenticationFailed(context.Method, context.Peer);
            throw new RpcException(new(StatusCode.PermissionDenied, "Permission Denied"));
        }

        if (!_authenticator.Authenticate(ticketEntry.Value)) {
            _logger.LogAuthenticationFailed(context.Method, context.Peer);
            throw new RpcException(new(StatusCode.PermissionDenied, "Permission Denied"));
        }

        return base.UnaryServerHandler(request, context, continuation);
    }

    private static string[] AllMethodFullPath(ServiceDescriptor descriptor) {
        return descriptor.Methods.Select((method) => $"/{descriptor.FullName}/{method.Name}").ToArray();
    }
}

public static partial class AuthenticatorInterceptorLogger {
    [LoggerMessage(EventId = 9401, Level = LogLevel.Warning, Message = "AuthenticateFailed in {method} from {peer}")]
    public static partial void LogAuthenticationFailed(this ILogger<AuthenticatorInterceptor> logger, string method, string peer);
}