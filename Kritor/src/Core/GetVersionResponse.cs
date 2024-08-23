namespace Kritor.Core;

public partial class GetVersionResponse {
    public GetVersionResponse SetVersion(string version) {
        Version = version;
        return this;
    }

    public GetVersionResponse SetAppName(string appName) {
        AppName = appName;
        return this;
    }
}