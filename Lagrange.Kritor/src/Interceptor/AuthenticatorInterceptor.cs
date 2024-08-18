using System.Linq;
using System.Threading.Tasks;
using Google.Protobuf.Reflection;
using Grpc.Core;
using Kritor.Authentication;
using Lagrange.Kritor.Utility;
using Microsoft.Extensions.Logging;
using GrpcInterceptor = Grpc.Core.Interceptors.Interceptor;

namespace Lagrange.Kritor.Interceptor;

public class AuthenticatorInterceptor(ILogger<AuthenticatorInterceptor> logger, Authenticator authenticator) : GrpcInterceptor {
    private readonly ILogger<AuthenticatorInterceptor> _logger = logger;

    private readonly Authenticator _authenticator = authenticator;

    private readonly RpcException _permissionDeniedException = new(new(StatusCode.PermissionDenied, "Permission Denied"));

    private readonly string[] _sikp = [
        ..AllMethodFullPath(AuthenticationService.Descriptor)
    ];

    public override Task<TResponse> UnaryServerHandler<TRequest, TResponse>(TRequest request, ServerCallContext context, UnaryServerMethod<TRequest, TResponse> continuation) {
        if (_sikp.Contains(context.Method)) return base.UnaryServerHandler(request, context, continuation);

        if (!_authenticator.IsEnabled) return base.UnaryServerHandler(request, context, continuation);

        Metadata.Entry? ticketEntry = context.RequestHeaders.Get("ticket");

        if (ticketEntry == null) {
            _logger.LogAuthenticationFailed(context.Method, context.Peer);
            throw _permissionDeniedException;
        }

        if (!_authenticator.Authenticate(ticketEntry.Value)) {
            _logger.LogAuthenticationFailed(context.Method, context.Peer);
            throw _permissionDeniedException;
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