namespace Kritor.Common;

public partial class XmlElement {
    public static XmlElement Create(string xml) {
        return new() { Xml = xml };
    }
}