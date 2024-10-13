using System.Threading.Tasks;
using Grpc.Core;
using Kritor.Common;
using static Kritor.Reverse.ReverseService;

namespace Lagrange.Kritor.Services.Kritor.Grpc.Reverse;

public class KritorReverseService : ReverseServiceBase {
    // WONTSUPPORTED
    public override Task ReverseStream(IAsyncStreamReader<Response> requestStream, IServerStreamWriter<Request> responseStream, ServerCallContext context) {
        return base.ReverseStream(requestStream, responseStream, context);
    }
}