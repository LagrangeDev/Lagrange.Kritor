using Google.Protobuf;

namespace Kritor.Common;

public partial class VoiceElement {
    public VoiceElement SetFile(ByteString file) {
        File = file;
        return this;
    }

    public VoiceElement SetFileName(string fileName) {
        FileName = fileName;
        return this;
    }

    public VoiceElement SetFilePath(string filePath) {
        FilePath = filePath;
        return this;
    }

    public VoiceElement SetFileUrl(string fileUrl) {
        FileUrl = fileUrl;
        return this;
    }

    public VoiceElement SetFileMd5(string fileMd5) {
        FileMd5 = fileMd5;
        return this;
    }

    public VoiceElement SetMagic(bool magic) {
        Magic = magic;
        return this;
    }
}