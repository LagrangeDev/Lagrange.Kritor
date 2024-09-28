using System.Net.Http;
using System.Threading.Tasks;

namespace Lagrange.Kritor.Utilities;

public static class HttpClientUtility {
    private static readonly HttpClient _client = new();

    public static Task<byte[]> GetBytes(string url) {
        return _client.GetByteArrayAsync(url);
    }
}