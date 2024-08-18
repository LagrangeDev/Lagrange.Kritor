using System.Threading.Tasks;
using Grpc.Core;
using Kritor.Core;

namespace Lagrange.Kritor.Service.Grpc;

public class KritorCoreService : CoreService.CoreServiceBase {
    public override Task<GetVersionResponse> GetVersion(GetVersionRequest request, ServerCallContext context) {
        return base.GetVersion(request, context);
    }
}