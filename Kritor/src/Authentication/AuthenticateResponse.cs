using static Kritor.Authentication.AuthenticateResponse.Types;

namespace Kritor.Authentication;

public partial class AuthenticateResponse {
    public static AuthenticateResponse CreateOk(string msg = "Ok") {
        return new() { Code = AuthenticateResponseCode.Ok, Msg = msg };
    }

    public static AuthenticateResponse CreateLogicError(string msg) {
        return new() { Code = AuthenticateResponseCode.LogicError, Msg = msg };
    }
}