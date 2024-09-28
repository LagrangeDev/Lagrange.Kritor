using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Grpc.Core;
using Kritor.File;
using Lagrange.Core;
using Lagrange.Core.Common.Entity;
using Lagrange.Core.Common.Interface.Api;
using Lagrange.Core.Message.Entity;
using Lagrange.Kritor.Utilities;
using static Kritor.File.GroupFileService;
using KritorFile = Kritor.File.File;

namespace Lagrange.Kritor.Services.Kritor.Grpc.File;

public class GroupFileService(BotContext bot) : GroupFileServiceBase {
    private readonly BotContext _bot = bot;

    public override Task<CreateFolderResponse> CreateFolder(CreateFolderRequest request, ServerCallContext context) {
        // (int retCode, string retMsg) = await _bot.GroupFSCreateFolder((uint)request.GroupId, request.Name);

        return base.CreateFolder(request, context);
    }

    public override Task<RenameFolderResponse> RenameFolder(RenameFolderRequest request, ServerCallContext context) {
        // (int retCode, string retMsg) = await _bot.GroupFSRenameFolder((uint)request.GroupId, request.FolderId, request.Name);

        return base.RenameFolder(request, context);
    }

    public override async Task<DeleteFolderResponse> DeleteFolder(DeleteFolderRequest request, ServerCallContext context) {
        (int retCode, string retMsg) = await _bot.GroupFSDeleteFolder((uint)request.GroupId, request.FolderId);

        if (retCode != 0) throw new Exception($"({retCode}) {retMsg}");

        return new DeleteFolderResponse { };
    }

    public override async Task<UploadFileResponse> UploadFile(UploadFileRequest request, ServerCallContext context) {
        FileEntity entity = request.DataCase switch {
            UploadFileRequest.DataOneofCase.None => throw new NotSupportedException(
                "Not supported DataOneofCase(None)"
            ),
            UploadFileRequest.DataOneofCase.File => new FileEntity([.. request.File], "Lagrange.Kritor Upload File"),
            UploadFileRequest.DataOneofCase.FileName => throw new NotImplementedException(
                $"FileName? Didn't understand what it was?"
            ),
            UploadFileRequest.DataOneofCase.FilePath => new FileEntity(request.FilePath),
            UploadFileRequest.DataOneofCase.FileUrl => new FileEntity(
                await HttpClientUtility.GetBytes(request.FileUrl),
                "Lagrange.Kritor Upload File"
            ),
            UploadFileRequest.DataOneofCase unknown => throw new NotSupportedException(
                $"Not supported DataOneofCase({unknown})"
            ),
        };


        if (!await _bot.GroupFSUpload((uint)request.GroupId, entity)) throw new Exception("Upload file failed");

        return new UploadFileResponse { };
    }

    public override async Task<DeleteFileResponse> DeleteFile(DeleteFileRequest request, ServerCallContext context) {
        (int retCode, string retMsg) = await _bot.GroupFSDelete((uint)request.GroupId, request.FileId);

        if (retCode != 0) throw new Exception($"({retCode}) {retMsg}");

        return new();
    }

    public override async Task<GetFileSystemInfoResponse> GetFileSystemInfo(GetFileSystemInfoRequest request, ServerCallContext context) {
        return new GetFileSystemInfoResponse {
            FileCount = await _bot.FetchGroupFSCount((uint)request.GroupId),
            // TotalCount =
            UsedSpace = (uint)await _bot.FetchGroupFSSpace((uint)request.GroupId),
            // TotalCount =
        };
    }

    public override async Task<GetFileListResponse> GetFileList(GetFileListRequest request, ServerCallContext context) {
        List<IBotFSEntry> entries = await _bot.FetchGroupFSList(
            (uint)request.GroupId,
            request.HasFolderId ? request.FolderId : "/"
        );

        return new GetFileListResponse {
            Files = {
                entries.OfType<BotFileEntry>()
                    .Select((entry) => new KritorFile {
                        FileId = entry.FileId,
                        FileName = entry.FileName,
                        FileSize = entry.FileSize,
                        // BusId =
                        UploadTime = (ulong)new DateTimeOffset(entry.UploadedTime).ToUnixTimeSeconds(),
                        ExpireTime = (ulong)new DateTimeOffset(entry.ExpireTime).ToUnixTimeSeconds(),
                        ModifyTime = (ulong)new DateTimeOffset(entry.ModifiedTime).ToUnixTimeSeconds(),
                        DownloadTimes = entry.DownloadedTimes,
                        Uploader = entry.UploaderUin,
                        // UploaderName =
                        // Sha =
                        // Sha3 =
                        // Md5 =
                    })
            },
            Folders = {
                entries.OfType<BotFolderEntry>()
                    .Select((entry) => new Folder {
                        FolderId = entry.FolderId,
                        FolderName = entry.FolderName,
                        TotalFileCount = entry.TotalFileCount,
                        CreateTime = (ulong)new DateTimeOffset(entry.CreateTime).ToUnixTimeSeconds(),
                        Creator = entry.CreatorUin,
                        // CreatorName = entry.
                    })
            }
        };
    }
}
