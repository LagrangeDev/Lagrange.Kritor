using Google.Protobuf;

namespace Kritor.Common;

public partial class VideoElement {
        public VideoElement SetFile(ByteString file) {
        File = file;
        return this;
    }

    public VideoElement SetFileName(string fileName) {
        FileName = fileName;
        return this;
    }

    public VideoElement SetFilePath(string filePath) {
        FilePath = filePath;
        return this;
    }

    public VideoElement SetFileUrl(string fileUrl) {
        FileUrl = fileUrl;
        return this;
    }
    
    public VideoElement SetFileMd5(string fileMd5) {
        FileMd5 = fileMd5;
        return this;
    }
}