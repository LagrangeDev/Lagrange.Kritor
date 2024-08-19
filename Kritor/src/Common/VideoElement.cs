namespace Kritor.Common;

public partial class VideoElement {
    public static VideoElement CreateVideoUrl(string fileUrl, string? fileMd5) {
        VideoElement video = new() { FileUrl = fileUrl };
        if (fileMd5 != null) video.FileMd5 = fileMd5;
        return video;
    }
}