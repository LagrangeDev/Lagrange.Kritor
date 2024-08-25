namespace Kritor.File;

public partial class GetFileListResponse {
    public GetFileListResponse AddFiles(params File[] files) {
        Files.Add(files);
        return this;
    }

    public GetFileListResponse AddFolders(params Folder[] folders) {
        Folders.Add(folders);
        return this;
    }
}