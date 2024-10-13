using System.Threading.Tasks;
using Grpc.Core;
using Kritor.Process;
using static Kritor.Process.ProcessService;

namespace Lagrange.Kritor.Services.Kritor.Grpc.Process;

public class KritorProcessService : ProcessServiceBase {
    // WAITIMPL: Kritor No RequestId
    public override Task<SetFriendApplyResultResponse> SetFriendApplyResult(SetFriendApplyResultRequest request, ServerCallContext context) {
        return base.SetFriendApplyResult(request, context);
    }

    // WAITIMPL: Kritor No RequestId
    public override Task<SetGroupApplyResultResponse> SetGroupApplyResult(SetGroupApplyResultRequest request, ServerCallContext context) {
        return base.SetGroupApplyResult(request, context);
    }

    // WAITIMPL: Kritor No RequestId
    public override Task<SetInvitedJoinGroupResultResponse> SetInvitedJoinGroupResult(SetInvitedJoinGroupResultRequest request, ServerCallContext context) {
        return base.SetInvitedJoinGroupResult(request, context);
    }
}