using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using Lagrange.Core.Utility.Sign;

namespace Lagrange.Kritor.Provider;

public class KritorSignerProvider(string? url) : SignProvider {
    private readonly byte[] _dummy = new byte[35];

    private readonly HttpClient _client = new();

    public override byte[]? Sign(string cmd, uint seq, byte[] body, out byte[]? e, out string? t) {
        // result
        e = null;
        t = null;

        if (!WhiteListCommand.Contains(cmd)) return null;

        if (url is null) throw new Exception("Sign server is not configured");

        using HttpRequestMessage request = new() {
            Method = HttpMethod.Post,
            RequestUri = new(url),
            Content = new StringContent(
                $"{{\"cmd\":\"{cmd}\",\"seq\":{seq},\"src\":\"{Convert.ToHexString(body)}\"}}",
                new MediaTypeHeaderValue("application/json")
            )
        };

        HttpResponseMessage message = _client.Send(request);

        if (message.StatusCode is not HttpStatusCode.OK) throw new Exception($"Signer server returned a {message.StatusCode}");

        JsonElement json = JsonDocument.Parse(message.Content.ReadAsStream()).RootElement;

        JsonElement valueJson = json.GetProperty("value");

        JsonElement extraJson = valueJson.GetProperty("extra");
        JsonElement tokenJson = valueJson.GetProperty("token");
        JsonElement signJson = valueJson.GetProperty("sign");

        string? extra = extraJson.GetString();
        e = extra is not null ? Convert.FromHexString(extra) : [];

        string? token = tokenJson.GetString();
        t = token is not null ? Encoding.UTF8.GetString(Convert.FromHexString(token)) : "";

        string sign = signJson.GetString() ?? throw new Exception("Signer server returned an empty sign");
        return Convert.FromHexString(sign);
    }
}