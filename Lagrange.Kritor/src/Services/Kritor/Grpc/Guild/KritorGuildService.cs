using System.Threading.Tasks;
using Grpc.Core;
using Kritor.Guild;
using static Kritor.Guild.GuildService;

namespace Lagrange.Kritor.Services.Kritor.Grpc.Guild;

public class KritorGuildService : GuildServiceBase {
    public override Task<GetBotInfoResponse> GetBotInfo(GetBotInfoRequest request, ServerCallContext context) {
        return base.GetBotInfo(request, context);
    }

    public override Task<GetChannelListResponse> GetChannelList(GetChannelListRequest request, ServerCallContext context) {
        return base.GetChannelList(request, context);
    }

    public override Task<GetGuildMetaByGuestResponse> GetGuildMetaByGuest(GetGuildMetaByGuestRequest request, ServerCallContext context) {
        return base.GetGuildMetaByGuest(request, context);
    }

    public override Task<GetGuildChannelListResponse> GetGuildChannelList(GetGuildChannelListRequest request, ServerCallContext context) {
        return base.GetGuildChannelList(request, context);
    }

    public override Task<GetGuildMemberListResponse> GetGuildMemberList(GetGuildMemberListRequest request, ServerCallContext context) {
        return base.GetGuildMemberList(request, context);
    }

    public override Task<GetGuildMemberResponse> GetGuildMember(GetGuildMemberRequest request, ServerCallContext context) {
        return base.GetGuildMember(request, context);
    }

    public override Task<SendChannelMessageResponse> SendChannelMessage(SendChannelMessageRequest request, ServerCallContext context) {
        return base.SendChannelMessage(request, context);
    }

    public override Task<GetGuildFeedListResponse> GetGuildFeedList(GetGuildFeedListRequest request, ServerCallContext context) {
        return base.GetGuildFeedList(request, context);
    }

    public override Task<GetGuildRoleListResponse> GetGuildRoleList(GetGuildRoleListRequest request, ServerCallContext context) {
        return base.GetGuildRoleList(request, context);
    }

    public override Task<DeleteGuildRoleResponse> DeleteGuildRole(DeleteGuildRoleRequest request, ServerCallContext context) {
        return base.DeleteGuildRole(request, context);
    }

    public override Task<SetGuildMemberRoleResponse> SetGuildMemberRole(SetGuildMemberRoleRequest request, ServerCallContext context) {
        return base.SetGuildMemberRole(request, context);
    }

    public override Task<UpdateGuildRoleResponse> UpdateGuildRole(UpdateGuildRoleRequest request, ServerCallContext context) {
        return base.UpdateGuildRole(request, context);
    }

    public override Task<CreateGuildRoleResponse> CreateGuildRole(CreateGuildRoleRequest request, ServerCallContext context) {
        return base.CreateGuildRole(request, context);
    }
}