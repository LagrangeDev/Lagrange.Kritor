namespace Kritor.File;

public partial class GetFileSystemInfoResponse {
    public GetFileSystemInfoResponse SetFileCount(uint fileCount) {
        FileCount = fileCount;
        return this;
    }

    public GetFileSystemInfoResponse SetTotalCount(uint totalCount) {
        TotalCount = totalCount;
        return this;
    }

    public GetFileSystemInfoResponse SetUsedSpace(uint usedSpace) {
        UsedSpace = usedSpace;
        return this;
    }

    public GetFileSystemInfoResponse SetTotalSpace(uint totalSpace) {
        TotalSpace = totalSpace;
        return this;
    }
}