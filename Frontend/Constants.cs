using Flurl.Http;
using Flurl.Http.Configuration;

namespace Testapplication1;

public class Constants
{
    public const string ChengetaToken = "ChengetaToken";
}

public class CookiesTokenBase
{
    protected readonly IFlurlClient _flurlClient;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CookiesTokenBase(IFlurlClientFactory flurlClientFactory,
        IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;

        _flurlClient = flurlClientFactory.Get("https://localhost:44347");

        _flurlClient.BeforeCall(flurlCall => 
        {
            var token = _httpContextAccessor.HttpContext.Request.Cookies[Constants.ChengetaToken];
            if (!string.IsNullOrWhiteSpace(token))
            {
                flurlCall.HttpRequestMessage.SetHeader("Authorization", $"bearer {token}");
            }
            else
            {
                flurlCall.HttpRequestMessage.SetHeader("Authorization", "");
            }
        });
    } 
}