using static Kritor.Authentication.AuthenticateResponse.Types;

namespace Kritor.Authentication;

public partial class AuthenticateResponse {
    private AuthenticateResponse(AuthenticateResponseCode code, string msg) {
        Code = code;
        Msg = msg;
    }

    public static AuthenticateResponse Ok(string msg = "Ok") {
        return new(AuthenticateResponseCode.Ok, msg);
    }
    
    public static AuthenticateResponse LogicError(string msg) {
        return new(AuthenticateResponseCode.LogicError, msg);
    }
}