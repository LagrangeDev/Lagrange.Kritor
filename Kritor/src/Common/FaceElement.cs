namespace Kritor.Common;

public partial class FaceElement {
    public FaceElement SetId(uint id) {
        Id = id;
        return this;
    }

    public FaceElement SetIsBig(bool isBig) {
        IsBig = isBig;
        return this;
    }

    public FaceElement SetResult(uint result) {
        Result = result;
        return this;
    }
}