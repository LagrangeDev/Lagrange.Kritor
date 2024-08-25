namespace Kritor.File;

public partial class CreateFolderResponse {
    public CreateFolderResponse SetId(string id) {
        Id = id;
        return this;
    }

    public CreateFolderResponse SetUsedSpace(ulong usedSpace) {
        UsedSpace = usedSpace;
        return this;
    }
}