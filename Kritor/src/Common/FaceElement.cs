namespace Kritor.Common;

public partial class FaceElement {
    private FaceElement(uint id) {
        Id = id;
    }

    public static FaceElement Create(uint id) {
        return new(id);
    }
}