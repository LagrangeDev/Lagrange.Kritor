namespace Kritor.Common;

public partial class TextElement {
    private TextElement(string text) {
        Text = text;
    }

    public static TextElement Create(string text) {
        return new(text);
    }
}