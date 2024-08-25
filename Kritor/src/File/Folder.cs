namespace Kritor.File;

public partial class Folder {
    public Folder SetFolderId(string folderId) {
        FolderId = folderId;
        return this;
    }

    public Folder SetFolderName(string folderName) {
        FolderName = folderName;
        return this;
    }

    public Folder SetTotalFileCount(uint totalFileCount) {
        TotalFileCount = totalFileCount;
        return this;
    }

    public Folder SetCreateTime(ulong createTime) {
        CreateTime = createTime;
        return this;
    }

    public Folder SetCreator(ulong creator) {
        Creator = creator;
        return this;
    }

    public Folder SetCreatorName(string creatorName) {
        CreatorName = creatorName;
        return this;
    }
}