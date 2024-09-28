using System.IO;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Lagrange.Kritor.Utilities;

public static class HttpClientUtility {
    private static readonly HttpClient _client = new();

    public static byte[] GetBytes(string url) {
        using HttpRequestMessage request = new(HttpMethod.Get, url);
        using HttpResponseMessage response = _client.Send(request);
        using HttpContent content = response.Content;
        using MemoryStream stream = new();
        content.CopyTo(stream, null, default);
        return stream.ToArray();
    }

    public static Task<byte[]> GetBytesAsync(string url, CancellationToken token) {
        return _client.GetByteArrayAsync(url, token);
    }
}