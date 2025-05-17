using Microsoft.Identity.Client;

namespace DocxToPdfConverter.Services;
public class DelegatedAuthenticationService(string clientId, string[] defaultScopes) : IAuthenticationService
{
    private readonly IPublicClientApplication _app = PublicClientApplicationBuilder
            .Create(clientId)
            .WithAuthority(AzureCloudInstance.AzurePublic, "consumers")
            .WithRedirectUri("http://localhost")
            .Build();
    private readonly string[] _defaultScopes = defaultScopes;

    public async Task<string> GetAccessTokenAsync(string[] scopes)
    {
        var accounts = await _app.GetAccountsAsync();
        AuthenticationResult result;

        try
        {
            result = await _app.AcquireTokenSilent(scopes, accounts.FirstOrDefault())
                .ExecuteAsync();
        }
        catch (MsalUiRequiredException)
        {
            // Interactive login required
            result = await _app.AcquireTokenInteractive(scopes)
                .WithUseEmbeddedWebView(false) // Use system browser
                .ExecuteAsync();
        }

        return result.AccessToken;
    }
}