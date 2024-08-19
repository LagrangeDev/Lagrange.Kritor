namespace Kritor.Authentication;

public partial class AddTicketResponse {
    public static AddTicketResponse CreateOk(string msg = "Ok") {
        return new() { Code = TicketOperationResponseCode.Ok, Msg = msg };
    }

    public static AddTicketResponse CreateError(string msg) {
        return new() { Code = TicketOperationResponseCode.Error, Msg = msg };
    }
}