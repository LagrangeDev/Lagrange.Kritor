using Google.Protobuf;
using static Kritor.Common.ImageElement.Types;

namespace Kritor.Common;

public partial class ImageElement {
    public static ImageElement CreateCommonImageUrl(string fileUrl, string? fileMd5, uint? subType) {
        ImageElement image = new() { FileType = ImageType.Common, FileUrl = fileUrl };
        if (fileMd5 != null) image.FileMd5 = fileMd5;
        if (subType.HasValue) image.SubType = subType.Value;
        return image;
    }
}