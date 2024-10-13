using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Lagrange.Kritor.Utilities;

public static class HttpClientUtility {
    private static readonly HttpClient client = new();

    public static Task<byte[]> GetBytesAsync(string url, CancellationToken token) {
        return client.GetByteArrayAsync(url, token);
    }
}