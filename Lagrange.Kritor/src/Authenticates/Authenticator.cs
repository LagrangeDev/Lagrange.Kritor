using System.Linq;

namespace Lagrange.Kritor.Authenticates;

public class Authenticator(bool enabled, string super, string uin, string[] tickets) {
    private readonly bool _enabled = enabled;

    private readonly string _super = super;

    private readonly string _uin = uin;

    private readonly string[] _tickets = tickets;

    public bool IsEnabled { get => _enabled; }

    public string[] Tickets { get => _tickets; }

    public bool Authenticate(string uin, string ticket) {
        return uin == _uin && (!_enabled || ticket == _super || _tickets.Contains(ticket));
    }

    public bool Authenticate(string ticket, bool isAdmin = false) {
        return !_enabled || ticket == _super || (!isAdmin && _tickets.Contains(ticket));
    }
}