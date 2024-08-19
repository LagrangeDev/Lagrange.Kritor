namespace Kritor.Common;

public partial class AtElement {
    public static AtElement Create(ulong uin) {
        return new() { Uin = uin };
    }
}