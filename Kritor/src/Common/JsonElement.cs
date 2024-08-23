namespace Kritor.Common;

public partial class JsonElement {
    public JsonElement SetJson(string json) {
        Json = json;
        return this;
    }
}