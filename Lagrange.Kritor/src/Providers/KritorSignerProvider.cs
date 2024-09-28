using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using Lagrange.Core.Utility.Sign;

namespace Lagrange.Kritor.Providers;

public class KritorSignerProvider(string? url, string? proxy) : SignProvider {
    private readonly string? _url = url;

    private readonly HttpClient _client = new(new HttpClientHandler {
        Proxy = string.IsNullOrEmpty(proxy) ? null : new WebProxy() {
            Address = new Uri(proxy),
            BypassProxyOnLocal = false,
            UseDefaultCredentials = false,
        },
    }, true);

    public override byte[]? Sign(string cmd, uint seq, byte[] body, out byte[]? e, out string? t) {
        // result
        e = null;
        t = null;

        if (!WhiteListCommand.Contains(cmd)) return null;

        if (_url == null) return null;

        using HttpRequestMessage request = new() {
            Method = HttpMethod.Post,
            RequestUri = new(_url),
            Content = new StringContent(
                $"{{\"cmd\":\"{cmd}\",\"seq\":{seq},\"src\":\"{Convert.ToHexString(body)}\"}}",
                new MediaTypeHeaderValue("application/json")
            )
        };

        HttpResponseMessage message = _client.Send(request);

        if (message.StatusCode != HttpStatusCode.OK) throw new Exception($"Signer server returned a {message.StatusCode}");

        JsonElement json = JsonDocument.Parse(message.Content.ReadAsStream()).RootElement;

        JsonElement valueJson = json.GetProperty("value");

        JsonElement extraJson = valueJson.GetProperty("extra");
        JsonElement tokenJson = valueJson.GetProperty("token");
        JsonElement signJson = valueJson.GetProperty("sign");

        string? extra = extraJson.GetString();
        e = extra != null ? Convert.FromHexString(extra) : [];

        string? token = tokenJson.GetString();
        t = token != null ? Encoding.UTF8.GetString(Convert.FromHexString(token)) : "";

        string sign = signJson.GetString() ?? throw new Exception("Signer server returned an empty sign");
        return Convert.FromHexString(sign);
    }
}