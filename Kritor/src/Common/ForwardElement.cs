namespace Kritor.Common;

public partial class ForwardElement {
    public ForwardElement SetResId(string resId) {
        ResId = resId;
        return this;
    }

    public ForwardElement SetUniseq(string uniseq) {
        Uniseq = uniseq;
        return this;
    }

    public ForwardElement SetSummary(string summary) {
        Summary = summary;
        return this;
    }

    public ForwardElement SetDescription(string description) {
        Description = description;
        return this;
    }
}