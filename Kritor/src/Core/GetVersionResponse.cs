namespace Kritor.Core;

public partial class GetVersionResponse {
    private GetVersionResponse(string version, string appName) {
        Version = version;
        AppName = appName;
    }

    public static GetVersionResponse Create(string version, string appName) {
        return new(version, appName);
    }
}