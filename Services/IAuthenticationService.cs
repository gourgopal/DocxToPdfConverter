namespace DocxToPdfConverter.Services;

public interface IAuthenticationService
{
    Task<string> GetAccessTokenAsync(string[] scopes);
}
