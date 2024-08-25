namespace Kritor.File;

public partial class File {
    public File SetFileId(string fileId) {
        FileId = fileId;
        return this;
    }

    public File SetFileName(string fileName) {
        FileName = fileName;
        return this;
    }

    public File SetFileSize(ulong fileSize) {
        FileSize = fileSize;
        return this;
    }

    public File SetBusId(int busId) {
        BusId = busId;
        return this;
    }

    public File SetUploadTime(ulong uploadTime) {
        UploadTime = uploadTime;
        return this;
    }

    public File SetExpireTime(ulong expireTime) {
        ExpireTime = expireTime;
        return this;
    }

    public File SetModifyTime(ulong modifyTime) {
        ModifyTime = modifyTime;
        return this;
    }

    public File SetDownloadTimes(uint downloadTimes) {
        DownloadTimes = downloadTimes;
        return this;
    }

    public File SetUploader(ulong uploader) {
        Uploader = uploader;
        return this;
    }

    public File SetUploaderName(string uploaderName) {
        UploaderName = uploaderName;
        return this;
    }

    public File SetSha(string sha) {
        Sha = sha;
        return this;
    }

    public File SetSha3(string sha3) {
        Sha3 = sha3;
        return this;
    }

    public File SetMd5(string md5) {
        Md5 = md5;
        return this;
    }
}