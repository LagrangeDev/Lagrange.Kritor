namespace Kritor.Authentication;

public partial class GetAuthenticationStateResponse {
    public GetAuthenticationStateResponse SetIsRequired(bool isRequired) {
        IsRequired = isRequired;
        return this;
    }
}