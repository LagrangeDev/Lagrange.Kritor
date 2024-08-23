namespace Kritor.Common;

public partial class ForwardElement {
    public static ForwardElement Create(string resId, string uniseq, string summary, string description) {
        return new ForwardElement() { ResId = resId, Uniseq = uniseq, Summary = summary, Description = description };
    }
}