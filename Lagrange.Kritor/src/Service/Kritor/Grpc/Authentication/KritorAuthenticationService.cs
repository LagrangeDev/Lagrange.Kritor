using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Grpc.Core;
using Kritor.Authentication;
using Lagrange.Kritor.Utility;
using Microsoft.Extensions.Logging;
using static Kritor.Authentication.AuthenticationService;

namespace Lagrange.Kritor.Service.Kritor.Grpc.Authentication;

public class KritorAuthenticationService(ILogger<KritorAuthenticationService> logger, Authenticator authenticator) : AuthenticationServiceBase {
    private readonly ILogger<KritorAuthenticationService> _logger = logger;

    private readonly Authenticator _authenticator = authenticator;

    public override Task<AuthenticateResponse> Authenticate(AuthenticateRequest request, ServerCallContext context) {
        if (!_authenticator.Authenticate(request.Account, request.Ticket)) {
            _logger.LogAuthenticationFailed(context.Peer);
            return Task.FromResult(AuthenticateResponse.LogicError("Account or Ticket does not match"));
        }

        return Task.FromResult(AuthenticateResponse.Ok());
    }

    public override Task<GetAuthenticationStateResponse> GetAuthenticationState(GetAuthenticationStateRequest request, ServerCallContext context) {
        return Task.FromResult(GetAuthenticationStateResponse.Create(_authenticator.IsEnabled));
    }

    public override Task<GetTicketResponse> GetTicket(GetTicketRequest request, ServerCallContext context) {
        if (!_authenticator.IsEnabled) return Task.FromResult(GetTicketResponse.Error("Authentication is not enabled"));

        if (_authenticator.Authenticate(request.SuperTicket, true)) {
            _logger.LogAuthenticationFailed(context.Peer);
            return Task.FromResult(GetTicketResponse.Error("Account or Ticket does not match"));
        }

        return Task.FromResult(GetTicketResponse.Ok(_authenticator.Tickets));
    }
}

public static partial class KritorAuthenticationServiceLogger {
    [LoggerMessage(EventId = 9401, Level = LogLevel.Error, Message = "AuthenticationAccountFailed in {method} from {peer}")]
    public static partial void LogAuthenticationFailed(this ILogger<KritorAuthenticationService> logger, string peer, [CallerMemberName] string method = "");
}