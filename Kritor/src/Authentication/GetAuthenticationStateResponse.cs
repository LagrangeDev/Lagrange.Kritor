namespace Kritor.Authentication;

public partial class GetAuthenticationStateResponse {
    public static GetAuthenticationStateResponse Create(bool isRequired) {
        return new() { IsRequired = isRequired };
    }
}