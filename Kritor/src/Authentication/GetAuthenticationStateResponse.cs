namespace Kritor.Authentication;

public partial class GetAuthenticationStateResponse {
    private GetAuthenticationStateResponse(bool isRequired) {
        IsRequired = isRequired;
    }

    public static GetAuthenticationStateResponse Create(bool isRequired) {
        return new GetAuthenticationStateResponse(isRequired);
    }
}