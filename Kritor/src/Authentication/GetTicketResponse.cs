using System.Collections.Generic;

namespace Kritor.Authentication;

public partial class GetTicketResponse {
    public GetTicketResponse AddTickets(IEnumerable<string> tickets) {
        Tickets.Add(tickets);
        return this;
    }

    public static GetTicketResponse CreateOk(string msg, string[] tickets) {
        return new GetTicketResponse() { Code = TicketOperationResponseCode.Ok, Msg = msg }.AddTickets(tickets);
    }

    public static GetTicketResponse CreateOk(string[] tickets) {
        return CreateOk("Ok", tickets);
    }

    public static GetTicketResponse CreateError(string msg) {
        return new() { Code = TicketOperationResponseCode.Error, Msg = msg };
    }
}