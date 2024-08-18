using System.Collections.Generic;

namespace Kritor.Authentication;

public partial class GetTicketResponse {
    private GetTicketResponse(TicketOperationResponseCode code, string msg, string[] tickets) {
        Code = code;
        Msg = msg;
        Tickets.Add(tickets);
    }

    public static GetTicketResponse Ok(string msg, string[] tickets) {
        return new(TicketOperationResponseCode.Ok, msg, tickets);
    }

    public static GetTicketResponse Ok(string[] tickets) {
        return Ok("Ok", tickets);
    }

    public static GetTicketResponse Error(string msg) {
        return new(TicketOperationResponseCode.Error, msg, []);
    }
}