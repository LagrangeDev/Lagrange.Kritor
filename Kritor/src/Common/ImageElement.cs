using Google.Protobuf;
using static Kritor.Common.ImageElement.Types;

namespace Kritor.Common;

public partial class ImageElement {
    public ImageElement SetFile(ByteString file) {
        File = file;
        return this;
    }

    public ImageElement SetFileName(string fileName) {
        FileName = fileName;
        return this;
    }

    public ImageElement SetFilePath(string filePath) {
        FilePath = filePath;
        return this;
    }

    public ImageElement SetFileUrl(string fileUrl) {
        FileUrl = fileUrl;
        return this;
    }
    
    public ImageElement SetFileMd5(string fileMd5) {
        FileMd5 = fileMd5;
        return this;
    }

    public ImageElement SetSubType(uint subType) {
        SubType = subType;
        return this;
    }

    public ImageElement SetFileType(ImageType fileType) {
        FileType = fileType;
        return this;
    }
}