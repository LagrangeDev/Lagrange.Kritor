using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Grpc.Core;
using Kritor.File;
using Lagrange.Core;
using Lagrange.Core.Common.Entity;
using Lagrange.Core.Common.Interface.Api;
using Lagrange.Core.Message.Entity;
using static Kritor.File.GroupFileService;
using KritorFile = Kritor.File.File;

namespace Lagrange.Kritor.Service.Kritor.Grpc.File;

public class GroupFileService(BotContext bot) : GroupFileServiceBase {
    private readonly BotContext _bot = bot;

    // TODO: Lagrange NotSupport Result
    // public override async Task<CreateFolderResponse> CreateFolder(CreateFolderRequest request, ServerCallContext context) {
    //     (int retcode, string message) = await _bot.GroupFSCreateFolder((uint)request.GroupId, request.Name);
    //     if (retcode != 0) throw new RpcException(new(StatusCode.Unknown, message));

    //     // _bot.FetchGroupFSSpace
    // }

    // TODO: Lagrange NotSupport
    // public override Task<RenameFolderResponse> RenameFolder(RenameFolderRequest request, ServerCallContext context) { }

    public override async Task<DeleteFolderResponse> DeleteFolder(DeleteFolderRequest request, ServerCallContext context) {
        (int retcode, string message) = await _bot.GroupFSDeleteFolder((uint)request.GroupId, request.FolderId);
        if (retcode != 0) throw new RpcException(new(StatusCode.Unknown, message));
        return new();
    }

    // public override Task<UploadFileResponse> UploadFile(UploadFileRequest request, ServerCallContext context) {
    //     MemoryStream stream = request.DataCase switch {
    //         UploadFileRequest.DataOneofCase.File => throw new System.NotImplementedException(),
    //         UploadFileRequest.DataOneofCase.FileName => throw new System.NotImplementedException(),
    //         UploadFileRequest.DataOneofCase.FilePath => throw new System.NotImplementedException(),
    //         UploadFileRequest.DataOneofCase.FileUrl => ,
    //         UploadFileRequest.DataOneofCase DataCase => throw new RpcException(new(StatusCode.InvalidArgument, $"`data` not suppeort {DataCase}")),
    //     };


    //     await _bot.GroupFSUpload(request.GroupId, new())

    //     stream.Close();
    // }

    public override async Task<DeleteFileResponse> DeleteFile(DeleteFileRequest request, ServerCallContext context) {
        (int retCode, string retMsg) = await _bot.GroupFSDelete((uint)request.GroupId, request.FileId);
        if (retCode != 0) throw new RpcException(new(StatusCode.Unknown, retMsg));
        return new();
    }

    // TODO: Lagrange NotSupport Max
    // public override Task<GetFileSystemInfoResponse> GetFileSystemInfo(GetFileSystemInfoRequest request, ServerCallContext context) { }

    // TODO: Lagrange Internal FileId
    // public override async Task<GetFileListResponse> GetFileList(GetFileListRequest request, ServerCallContext context) {
    //     List<IBotFSEntry> entries = await _bot.FetchGroupFSList(
    //         (uint)request.GroupId,
    //         request.HasFolderId ? request.FolderId : "/"
    //     );

    //     GetFileListResponse result = new();
    //     foreach (IBotFSEntry entry in entries) {
    //         switch (entry) {
    //             case BotFileEntry file: {
    //                 result.AddFiles(new KritorFile()
    //                     .SetFileId(file.)
    //                 );
    //                 break;
    //             }
    //             case BotFolderEntry: {
    //                 break;
    //             }
    //         }
    //     }
    // }
}
