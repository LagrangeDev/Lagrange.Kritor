namespace Kritor.Authentication;

public partial class AddTicketResponse {
    private AddTicketResponse(TicketOperationResponseCode code, string msg) {
        Code = code;
        Msg = msg;
    }

    public static AddTicketResponse Ok(string msg = "Ok") {
        return new(TicketOperationResponseCode.Ok, msg);
    }

    public static AddTicketResponse Error(string msg) {
        return new(TicketOperationResponseCode.Error, msg);
    }
}