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

    private readonly ReaderWriterLockSlim _lock;

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

        _lock = new();
    }

    public override Task<AuthenticateResponse> Authenticate(AuthenticateRequest request, ServerCallContext context) {
        if (_enabled) return Task.FromResult(AuthenticateResponse.Ok());

        if (request.Account != _uin) {
            _logger.LogAuthenticationAccountFailed(context.Peer);
            return Task.FromResult(AuthenticateResponse.LogicError("Account or Ticket does not match"));
        }

        if (request.Ticket != _super) {
            _lock.EnterReadLock();
            try {
                if (_tickets.Contains(request.Ticket)) {
                    _logger.LogAuthenticateTicketFiled(context.Peer);
                    return Task.FromResult(AuthenticateResponse.LogicError("Account or Ticket does not match"));
                }
            } finally { _lock.ExitReadLock(); }
        }

        return Task.FromResult(AuthenticateResponse.Ok());
    }

    public override Task<GetAuthenticationStateResponse> GetAuthenticationState(GetAuthenticationStateRequest request, ServerCallContext context) {
        return Task.FromResult(GetAuthenticationStateResponse.Create(_enabled));
    }

    public override Task<GetTicketResponse> GetTicket(GetTicketRequest request, ServerCallContext context) {
        if (!_enabled) return Task.FromResult(GetTicketResponse.Error("Authentication is not enabled"));

        if (request.Account != _uin) {
            _logger.LogAuthenticationAccountFailed(context.Peer);
            return Task.FromResult(GetTicketResponse.Error("Account does not match"));
        }

        if (request.SuperTicket != _super) {
            _logger.LogAuthenticateTicketFiled(context.Peer);
            return Task.FromResult(GetTicketResponse.Error("Account does not match"));
        }

        _lock.EnterReadLock();
        try {
            return Task.FromResult(GetTicketResponse.Ok(_tickets));
        } finally { _lock.ExitReadLock(); }
    }

    public override async Task<AddTicketResponse> AddTicket(AddTicketRequest request, ServerCallContext context) {
        if (!_enabled) return AddTicketResponse.Error("Authentication is not enabled");

        if (request.SuperTicket != _super) {
            _logger.LogAuthenticateTicketFiled(context.Peer);
            return AddTicketResponse.Error("SuperTicket does not match");
        }

        if (request.Account != _uin) return AddTicketResponse.Error("Account does not match");

        _lock.EnterReadLock();
        try {
            _tickets.Add(request.Ticket);

            Stream stream = File.Open("appsettings.json", FileMode.Open);
            JsonObject json = JsonSerializer.Deserialize<JsonObject>(stream)
                ?? throw new Exception("Unable to parse the appsettings.json file");

            JsonNode? kritor = json["Kritor"];
            if (kritor == null) {
                kritor = new JsonObject();
                json["Kritor"] = kritor;
            }

            JsonNode? authentication = kritor["Authentication"];
            if (authentication == null) {
                authentication = new JsonObject();
                kritor["Authentication"] = authentication;
            }

            authentication["Tickets"] = new JsonArray(_tickets.Select(ticket => JsonValue.Create(ticket)).ToArray());

            await stream.WriteAsync(Encoding.UTF8.GetBytes(json.ToJsonString()));

            return AddTicketResponse.Ok();
        } catch (Exception e) {
            _tickets.Remove(request.Ticket);
            _logger.LogAddTicketFiled(e);
            return AddTicketResponse.Error("Check the Lagrange.Core log for details.");
        } finally { _lock.ExitReadLock(); }
    }
}

public static partial class KritorAuthenticationServiceLogger {
    [LoggerMessage(EventId = 997, Level = LogLevel.Error, Message = "AddTicketFiled")]
    public static partial void LogAddTicketFiled(this ILogger<KritorAuthenticationService> logger, Exception e);

    [LoggerMessage(EventId = 998, Level = LogLevel.Error, Message = "AuthenticationTicketFailed in {method} from {peer}")]
    public static partial void LogAuthenticateTicketFiled(this ILogger<KritorAuthenticationService> logger, string peer, [CallerMemberName] string method = "");

    [LoggerMessage(EventId = 999, Level = LogLevel.Error, Message = "AuthenticationAccountFailed in {method} from {peer}")]
    public static partial void LogAuthenticationAccountFailed(this ILogger<KritorAuthenticationService> logger, string peer, [CallerMemberName] string method = "");
}