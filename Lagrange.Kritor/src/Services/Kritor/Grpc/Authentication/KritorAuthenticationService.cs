using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Grpc.Core;
using Kritor.Authentication;
using Lagrange.Kritor.Authenticates;
using Microsoft.Extensions.Logging;
using static Kritor.Authentication.AuthenticationService;

namespace Lagrange.Kritor.Services.Kritor.Grpc.Authentication;

public class KritorAuthenticationService(ILogger<KritorAuthenticationService> logger, Authenticator authenticator) : AuthenticationServiceBase {
    private readonly ILogger<KritorAuthenticationService> _logger = logger;

    private readonly Authenticator _authenticator = authenticator;

    public override Task<AuthenticateResponse> Authenticate(AuthenticateRequest request, ServerCallContext context) {
        if (!_authenticator.Authenticate(request.Account, request.Ticket)) {
            _logger.LogAuthenticationFailed(context.Peer);
            return Task.FromResult(new AuthenticateResponse {
                Code = AuthenticateResponse.Types.AuthenticateResponseCode.LogicError,
                Msg = "Account or Ticket does not match"
            });
        }

        return Task.FromResult(new AuthenticateResponse {
            Code = AuthenticateResponse.Types.AuthenticateResponseCode.Ok
        });
    }

    public override Task<GetAuthenticationStateResponse> GetAuthenticationState(GetAuthenticationStateRequest request, ServerCallContext context) {
        return Task.FromResult(new GetAuthenticationStateResponse { IsRequired = _authenticator.IsEnabled });
    }

    public override Task<GetTicketResponse> GetTicket(GetTicketRequest request, ServerCallContext context) {
        if (!_authenticator.IsEnabled) {
            return Task.FromResult(new GetTicketResponse {
                Code = TicketOperationResponseCode.Error,
                Msg = "Authentication is not enabled"
            });
        }

        if (_authenticator.Authenticate(request.SuperTicket, true)) {
            _logger.LogAuthenticationFailed(context.Peer);
            return Task.FromResult(new GetTicketResponse {
                Code = TicketOperationResponseCode.Error,
                Msg = "Account or Ticket does not match"
            });
        }

        return Task.FromResult(new GetTicketResponse {
            Code = TicketOperationResponseCode.Ok,
            Tickets = { _authenticator.Tickets }
        });
    }

    // WONTSUPPORTED
    public override Task<AddTicketResponse> AddTicket(AddTicketRequest request, ServerCallContext context) {
        return base.AddTicket(request, context);
    }

    // WONTSUPPORTED
    public override Task<DeleteTicketResponse> DeleteTicket(DeleteTicketRequest request, ServerCallContext context) {
        return base.DeleteTicket(request, context);
    }
}

public static partial class KritorAuthenticationServiceLogger {
    [LoggerMessage(EventId = 9401, Level = LogLevel.Error, Message = "AuthenticationAccountFailed in {method} from {peer}")]
    public static partial void LogAuthenticationFailed(this ILogger<KritorAuthenticationService> logger, string peer, [CallerMemberName] string method = "");
}