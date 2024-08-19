namespace Kritor.Common;

public partial class VoiceElement {
    public static VoiceElement CreateVoiceUrl(string fileUrl, string? fileMd5, bool? magic) {
        VoiceElement voice = new() { FileUrl = fileUrl };
        if (fileMd5 != null) voice.FileMd5 = fileMd5;
        if (magic.HasValue) voice.Magic = magic.Value;
        return voice;
    }
}