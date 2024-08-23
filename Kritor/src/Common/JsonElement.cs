namespace Kritor.Common;

public partial class JsonElement {
    public static JsonElement Create(string json) {
        return new() { Json = json };
    }
}