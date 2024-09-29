using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Grpc.Core;
using Kritor.Group;
using Lagrange.Core;
using Lagrange.Core.Common.Entity;
using Lagrange.Core.Common.Interface.Api;
using static Kritor.Group.GroupService;

namespace Lagrange.Kritor.Services.Kritor.Grpc.Group;

public class KritorGroupService(BotContext bot) : GroupServiceBase {
    private readonly BotContext _bot = bot;

    public override async Task<BanMemberResponse> BanMember(BanMemberRequest request, ServerCallContext context) {
        if (request.TargetCase != BanMemberRequest.TargetOneofCase.TargetUin) {
            throw new NotSupportedException($"Not supported TargetOneofCase({request.TargetCase})");
        }

        if (!await _bot.MuteGroupMember((uint)request.GroupId, (uint)request.TargetUin, request.Duration)) {
            throw new Exception($"Ban member failed");
        }

        return new BanMemberResponse { };
    }

    public override async Task<PokeMemberResponse> PokeMember(PokeMemberRequest request, ServerCallContext context) {
        if (request.TargetCase != PokeMemberRequest.TargetOneofCase.TargetUin) {
            throw new NotSupportedException($"Not supported TargetOneofCase({request.TargetCase})");
        }

        if (!await _bot.GroupPoke((uint)request.GroupId, (uint)request.TargetUin)) {
            throw new Exception($"Poke member failed");
        }

        return new PokeMemberResponse { };
    }

    public override async Task<KickMemberResponse> KickMember(KickMemberRequest request, ServerCallContext context) {
        if (request.TargetCase != KickMemberRequest.TargetOneofCase.TargetUin) {
            throw new NotSupportedException($"Not supported TargetOneofCase({request.TargetCase})");
        }

        if (!await _bot.KickGroupMember(
            (uint)request.GroupId,
            (uint)request.TargetUin,
            request.HasRejectAddRequest && request.RejectAddRequest
        // request.KickReason https://github.com/LagrangeDev/Lagrange.Core/pull/615
        )) throw new Exception($"Poke member failed");

        return new KickMemberResponse { };
    }

    public override async Task<LeaveGroupResponse> LeaveGroup(LeaveGroupRequest request, ServerCallContext context) {
        if (!await _bot.LeaveGroup((uint)request.GroupId)) throw new Exception("Leave group failed");

        return new LeaveGroupResponse { };
    }

    public override async Task<ModifyMemberCardResponse> ModifyMemberCard(ModifyMemberCardRequest request, ServerCallContext context) {
        if (request.TargetCase != ModifyMemberCardRequest.TargetOneofCase.TargetUin) {
            throw new NotSupportedException($"Not supported TargetOneofCase({request.TargetCase})");
        }

        if (!await _bot.RenameGroupMember((uint)request.GroupId, (uint)request.TargetUin, request.Card)) {
            throw new Exception($"Modify member card failed");
        }

        return new ModifyMemberCardResponse { };
    }

    public override async Task<ModifyGroupNameResponse> ModifyGroupName(ModifyGroupNameRequest request, ServerCallContext context) {
        if (!await _bot.RenameGroup((uint)request.GroupId, request.GroupName)) {
            throw new Exception($"Modify group name failed");
        }

        return new ModifyGroupNameResponse { };
    }

    public override async Task<ModifyGroupRemarkResponse> ModifyGroupRemark(ModifyGroupRemarkRequest request, ServerCallContext context) {
        if (!await _bot.RemarkGroup((uint)request.GroupId, request.Remark)) {
            throw new Exception($"Modify group remark failed");
        }

        return new ModifyGroupRemarkResponse { };
    }

    public override async Task<SetGroupAdminResponse> SetGroupAdmin(SetGroupAdminRequest request, ServerCallContext context) {
        if (request.TargetCase != SetGroupAdminRequest.TargetOneofCase.TargetUin) {
            throw new NotSupportedException($"Not supported TargetOneofCase({request.TargetCase})");
        }

        if (!await _bot.SetGroupAdmin((uint)request.GroupId, (uint)request.TargetUin, request.IsAdmin)) {
            throw new Exception($"Set group admin failed");
        }

        return new SetGroupAdminResponse { };
    }

    public override async Task<SetGroupUniqueTitleResponse> SetGroupUniqueTitle(SetGroupUniqueTitleRequest request, ServerCallContext context) {
        if (request.TargetCase != SetGroupUniqueTitleRequest.TargetOneofCase.TargetUin) {
            throw new NotSupportedException($"Not supported TargetOneofCase({request.TargetCase})");
        }

        if (!await _bot.GroupSetSpecialTitle((uint)request.GroupId, (uint)request.TargetUin, request.UniqueTitle)) {
            throw new Exception($"Set group unique title failed");
        }

        return new SetGroupUniqueTitleResponse { };
    }

    public override async Task<SetGroupWholeBanResponse> SetGroupWholeBan(SetGroupWholeBanRequest request, ServerCallContext context) {
        if (!await _bot.MuteGroupGlobal((uint)request.GroupId, request.IsBan)) {
            throw new Exception($"Set group whole ban failed");
        }

        return new SetGroupWholeBanResponse { };
    }

    public override async Task<GetGroupInfoResponse> GetGroupInfo(GetGroupInfoRequest request, ServerCallContext context) {
        List<BotGroup> groups = await _bot.FetchGroups(false);
        BotGroup group = groups.FirstOrDefault((group) => group.GroupUin == request.GroupId)
            ?? throw new Exception($"Group({request.GroupId}) does not exist");

        return new GetGroupInfoResponse {
            GroupInfo = new GroupInfo {
                GroupId = group.GroupUin,
                GroupName = group.GroupName,
                // GroupRemark =
                // Owner =
                // Admins =
                MaxMemberCount = group.MaxMember,
                MemberCount = group.MemberCount,
                GroupUin = group.GroupUin
            }
        };
    }

    public override async Task<GetGroupListResponse> GetGroupList(GetGroupListRequest request, ServerCallContext context) {
        List<BotGroup> groups = await _bot.FetchGroups(request.Refresh);

        IEnumerable<GroupInfo> infos = groups.Select(group => new GroupInfo {
            GroupId = group.GroupUin,
            GroupName = group.GroupName,
            // GroupRemark =
            // Owner =
            // Admins =
            MaxMemberCount = group.MaxMember,
            MemberCount = group.MemberCount,
            GroupUin = group.GroupUin
        });

        return new GetGroupListResponse {
            GroupsInfo = { infos }
        };
    }

    public override async Task<GetGroupMemberInfoResponse> GetGroupMemberInfo(GetGroupMemberInfoRequest request, ServerCallContext context) {
        if (request.TargetCase != GetGroupMemberInfoRequest.TargetOneofCase.TargetUin) {
            throw new NotSupportedException($"Not supported TargetOneofCase({request.TargetCase})");
        }

        List<BotGroupMember> members = await _bot.FetchMembers(
            (uint)request.GroupId,
            request.HasRefresh && request.Refresh
        );

        BotGroupMember member = members.FirstOrDefault(member => member.Uin == request.TargetUin)
            ?? throw new Exception(
                $"GroupMember(GroupId {request.GroupId} TargetUin {request.TargetUin}) does not exist"
            );

        return new GetGroupMemberInfoResponse {
            GroupMemberInfo = new GroupMemberInfo {
                Uin = member.Uin,
                Nick = member.MemberName,
                // Age =
                // UniqueTitle =
                // UniqueTitleExpireTime =
                Card = member.MemberCard,
                JoinTime = (ulong)new DateTimeOffset(member.JoinTime).ToUnixTimeSeconds(),
                LastActiveTime = (ulong)new DateTimeOffset(member.LastMsgTime).ToUnixTimeSeconds(),
                Level = member.GroupLevel,
                // ShutUpTime =
                // Distance =
                // Honors =
                // Unfriendly =
                // CardChangeable =
            }
        };
    }

    public override async Task<GetGroupMemberListResponse> GetGroupMemberList(GetGroupMemberListRequest request, ServerCallContext context) {
        List<BotGroupMember> members = await _bot.FetchMembers(
            (uint)request.GroupId,
            request.HasRefresh && request.Refresh
        );

        IEnumerable<GroupMemberInfo> infos = members.Select(member => new GroupMemberInfo {
            Uin = member.Uin,
            Nick = member.MemberName,
            // Age =
            // UniqueTitle =
            // UniqueTitleExpireTime =
            Card = member.MemberCard,
            JoinTime = (ulong)new DateTimeOffset(member.JoinTime).ToUnixTimeSeconds(),
            LastActiveTime = (ulong)new DateTimeOffset(member.LastMsgTime).ToUnixTimeSeconds(),
            Level = member.GroupLevel,
            // ShutUpTime =
            // Distance =
            // Honors =
            // Unfriendly =
            // CardChangeable =
        });

        return new GetGroupMemberListResponse {
            GroupMembersInfo = { infos }
        };
    }

    public override Task<GetProhibitedUserListResponse> GetProhibitedUserList(GetProhibitedUserListRequest request, ServerCallContext context) {
        return base.GetProhibitedUserList(request, context);
    }

    public override Task<GetRemainCountAtAllResponse> GetRemainCountAtAll(GetRemainCountAtAllRequest request, ServerCallContext context) {
        return base.GetRemainCountAtAll(request, context);
    }

    public override Task<GetNotJoinedGroupInfoResponse> GetNotJoinedGroupInfo(GetNotJoinedGroupInfoRequest request, ServerCallContext context) {
        return base.GetNotJoinedGroupInfo(request, context);
    }

    public override Task<GetGroupHonorResponse> GetGroupHonor(GetGroupHonorRequest request, ServerCallContext context) {
        return base.GetGroupHonor(request, context);
    }

    public override Task<UploadGroupFileResponse> UploadGroupFile(UploadGroupFileRequest request, ServerCallContext context) {
        // await _bot.GroupFSUpload((uint)request.GroupId, new FileEntity(MSFile.ReadAllBytes(request.File), request.Name));
        return base.UploadGroupFile(request, context);
    }
}