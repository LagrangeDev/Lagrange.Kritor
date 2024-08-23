namespace Kritor.Common;

public partial class XmlElement {
    public XmlElement SetXml(string xml) {
        Xml = xml;
        return this;
    }
}