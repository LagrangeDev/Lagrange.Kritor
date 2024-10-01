using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Grpc.Core;
using Kritor.Web;
using Lagrange.Core;
using Lagrange.Core.Common.Interface.Api;
using static Kritor.Web.WebService;

namespace Lagrange.Kritor.Services.Kritor.Grpc.Web;

public class KritorWebService(BotContext bot) : WebServiceBase {
    private readonly BotContext _bot = bot;

    public override async Task<GetCookiesResponse> GetCookies(GetCookiesRequest request, ServerCallContext context) {
        string domain = request.HasDomain ? request.Domain : throw new Exception("Domain is null");
        List<string> cookies = await _bot.FetchCookies([domain]);

        if (cookies.Count == 0) throw new Exception("Fetch cookies failed");

        return new GetCookiesResponse {
            Cookie = cookies[0]
        };
    }

    // TODO: Need to look into it. (；′⌒`)
    public override Task<GetCredentialsResponse> GetCredentials(GetCredentialsRequest request, ServerCallContext context) {
        return base.GetCredentials(request, context);
    }

    // TODO: Need to look into it. (；′⌒`)
    public override Task<GetCSRFTokenResponse> GetCSRFToken(GetCSRFTokenRequest request, ServerCallContext context) {
        return base.GetCSRFToken(request, context);
    }

    // TODO: Need to look into it. (；′⌒`)
    public override Task<GetHttpCookiesResponse> GetHttpCookies(GetHttpCookiesRequest request, ServerCallContext context) {
        return base.GetHttpCookies(request, context);
    }
}