namespace Kritor.Authentication;

public partial class GetTicketResponse {
    public GetTicketResponse SetCode(TicketOperationResponseCode code) {
        Code = code;
        return this;
    }

    public GetTicketResponse SetMsg(string msg) {
        Msg = msg;
        return this;
    }

    public GetTicketResponse AddTickets(params string[] tickets) {
        Tickets.Add(tickets);
        return this;
    }
}