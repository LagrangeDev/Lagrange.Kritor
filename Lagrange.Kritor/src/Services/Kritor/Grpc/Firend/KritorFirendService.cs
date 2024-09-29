using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Grpc.Core;
using Kritor.Friend;
using Lagrange.Core;
using Lagrange.Core.Common.Entity;
using Lagrange.Core.Common.Interface.Api;
using static Kritor.Friend.FriendService;

namespace Lagrange.Kritor.Services.Kritor.Grpc.Friend;

public class KritorFirendService(BotContext bot) : FriendServiceBase {
    private readonly BotContext _bot = bot;

    public override async Task<GetFriendListResponse> GetFriendList(GetFriendListRequest request, ServerCallContext context) {
        List<BotFriend> friends = await _bot.FetchFriends(request.HasRefresh && request.Refresh);

        IEnumerable<FriendInfo> infos = friends.Select(friend => new FriendInfo {
            Uin = friend.Uin,
            Qid = friend.Qid,
            Nick = friend.Nickname,
            Remark = friend.Remarks,
            // Level =
            // Age =
            // VoteCnt =
            // Gender =
            FriendGroupId = (int)friend.Group.GroupId
        });

        return new GetFriendListResponse {
            FriendsInfo = { infos }
        };
    }

    public override async Task<GetFriendProfileCardResponse> GetFriendProfileCard(GetFriendProfileCardRequest request, ServerCallContext context) {
        List<BotFriend> friends = await _bot.FetchFriends(false);

        IEnumerable<ProfileCard> profiles = friends.Where(friend => request.TargetUins.Contains(friend.Uin))
            .Select((friend) => new ProfileCard {
                Uin = friend.Uin,
                Qid = friend.Qid,
                Nick = friend.Nickname,
                Remark = friend.Remarks,
                // Level =
                // Birthday =
                // LoginDay =
                // VoteCnt =
                // IsSchoolVerified =
            });

        return new GetFriendProfileCardResponse {
            FriendsProfileCard = { profiles }
        };
    }

    public override async Task<GetStrangerProfileCardResponse> GetStrangerProfileCard(GetStrangerProfileCardRequest request, ServerCallContext context) {
        List<ProfileCard> profiles = [];
        foreach (var uin in request.TargetUins) {
            BotUserInfo? user = await _bot.FetchUserInfo((uint)uin, false);
            if (user == null) continue;
            profiles.Add(new ProfileCard {
                Uin = user.Uin,
                Qid = user.Qid,
                Nick = user.Nickname,
                // Remark =
                Level = user.Level,
                Birthday = (ulong)new DateTimeOffset(user.Birthday).ToUnixTimeSeconds(),
                // LoginDay =
                // VoteCnt =
                IsSchoolVerified = string.IsNullOrWhiteSpace(user.School)
            });
        }

        return new GetStrangerProfileCardResponse {
            StrangersProfileCard = { profiles }
        };
    }

    public override Task<SetProfileCardResponse> SetProfileCard(SetProfileCardRequest request, ServerCallContext context) {
        return base.SetProfileCard(request, context);
    }

    public override Task<IsBlackListUserResponse> IsBlackListUser(IsBlackListUserRequest request, ServerCallContext context) {
        return base.IsBlackListUser(request, context);
    }

    public override async Task<VoteUserResponse> VoteUser(VoteUserRequest request, ServerCallContext context) {
        if (request.HasTargetUin) throw new NotSupportedException($"Not support uin is null");

        if (!await _bot.Like((uint)request.TargetUin, request.VoteCount)) throw new Exception("Vote user failed");

        return new VoteUserResponse { };
    }

    public override Task<GetUidByUinResponse> GetUidByUin(GetUidByUinRequest request, ServerCallContext context) {
        return base.GetUidByUin(request, context);
    }

    public override Task<GetUinByUidResponse> GetUinByUid(GetUinByUidRequest request, ServerCallContext context) {
        return base.GetUinByUid(request, context);
    }

    public override Task<UploadPrivateChatFileResponse> UploadPrivateFile(PrivateChatFileRequest request, ServerCallContext context) {
        // bool v = await _bot.UploadFriendFile((uint)request.TargetUin, new FileEntity(request.File));

        return base.UploadPrivateFile(request, context);
    }
}