using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Threading;
using System.Threading.Tasks;
using Grpc.Core;
using Kritor.Authentication;
using Lagrange.Core;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using static Kritor.Authentication.AuthenticationService;

namespace Lagrange.Kritor.Service.Grpc.Kritor.Authentication;

public class KritorAuthenticationService : AuthenticationServiceBase {
    private readonly ILogger<KritorAuthenticationService> _logger;

    private readonly bool _enabled;

    private readonly string _super;

    private readonly string _uin;

    private readonly List<string> _tickets;

    public KritorAuthenticationService(ILogger<KritorAuthenticationService> logger, IConfiguration rootConfig, BotContext bot) {
        _logger = logger;

        IConfigurationSection config = rootConfig.GetRequiredSection("Kritor").GetRequiredSection("Authentication");

        _enabled = config.GetRequiredSection("Enabled").Get<bool>();

        _super = _enabled
            ? config.GetRequiredSection("SuperTicket").Get<string>()
                ?? throw new Exception("When Enabled is true, SuperTicket cannot be null")
            : "";

        _uin = bot.BotUin.ToString();

        _tickets = _enabled
            ? config.GetRequiredSection("Tickets").GetChildren()
                .Select((tickets) => {
                    return tickets.Get<string>() ?? throw new Exception("When Enabled is true, Tickets cannot be null");
                }).ToList()
            : [];
    }

    public override Task<AuthenticateResponse> Authenticate(AuthenticateRequest request, ServerCallContext context) {
        if (request.Account != _uin) {
            _logger.LogAuthenticationAccountFailed(context.Peer);
            return Task.FromResult(AuthenticateResponse.LogicError("Account or Ticket does not match"));
        }

        if (!_enabled) return Task.FromResult(AuthenticateResponse.Ok());

        if (request.Ticket != _super) {
            if (!_tickets.Contains(request.Ticket)) {
                _logger.LogAuthenticateTicketFiled(context.Peer);
                return Task.FromResult(AuthenticateResponse.LogicError("Account or Ticket does not match"));
            }
        }

        return Task.FromResult(AuthenticateResponse.Ok());
    }

    public override Task<GetAuthenticationStateResponse> GetAuthenticationState(GetAuthenticationStateRequest request, ServerCallContext context) {
        return Task.FromResult(GetAuthenticationStateResponse.Create(_enabled));
    }

    public override Task<GetTicketResponse> GetTicket(GetTicketRequest request, ServerCallContext context) {
        if (!_enabled) return Task.FromResult(GetTicketResponse.Error("Authentication is not enabled"));

        if (request.SuperTicket != _super) {
            _logger.LogAuthenticateTicketFiled(context.Peer);
            return Task.FromResult(GetTicketResponse.Error("SuperTicket does not match"));
        }

        if (request.Account != _uin) {
            _logger.LogAuthenticationAccountFailed(context.Peer);
            return Task.FromResult(GetTicketResponse.Error("Account does not match"));
        }

        return Task.FromResult(GetTicketResponse.Ok(_tickets));
    }

    // TODO: AddTicket

    // TODO: DeleteTicket
}

public static partial class KritorAuthenticationServiceLogger {
    [LoggerMessage(EventId = 998, Level = LogLevel.Error, Message = "AuthenticationTicketFailed in {method} from {peer}")]
    public static partial void LogAuthenticateTicketFiled(this ILogger<KritorAuthenticationService> logger, string peer, [CallerMemberName] string method = "");

    [LoggerMessage(EventId = 999, Level = LogLevel.Error, Message = "AuthenticationAccountFailed in {method} from {peer}")]
    public static partial void LogAuthenticationAccountFailed(this ILogger<KritorAuthenticationService> logger, string peer, [CallerMemberName] string method = "");
}