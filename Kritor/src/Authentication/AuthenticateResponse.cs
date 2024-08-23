using static Kritor.Authentication.AuthenticateResponse.Types;

namespace Kritor.Authentication;

public partial class AuthenticateResponse {
    public AuthenticateResponse SetCode(AuthenticateResponseCode code) {
        Code = code;
        return this;
    }

    public AuthenticateResponse SetMsg(string msg) {
        Msg = msg;
        return this;
    }
}