using Newtonsoft.Json;
using System.Net.Http.Headers;

namespace DocxToPdfConverter.Services;
public class GraphFileService(IAuthenticationService authenticationService) : IFileService
{
    private readonly IAuthenticationService _authenticationService = authenticationService;
    private HttpClient? _httpClient;

    private async Task<HttpClient> CreateAuthorizedHttpClient(string[] scopes)
    {
        if (_httpClient != null)
            return _httpClient;

        var token = await _authenticationService.GetAccessTokenAsync(scopes);
        _httpClient = new HttpClient();
        _httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");
        _httpClient.Timeout = TimeSpan.FromSeconds(600);

        return _httpClient;
    }

    public async Task<string> UploadFileAsync(string path, Stream content, string contentType)
    {
        var httpClient = await CreateAuthorizedHttpClient(["Files.ReadWrite"]);

        // Create a temporary filename with appropriate extension
        string fileExtension = contentType.Contains("openxmlformats") ? ".docx" : ".doc";
        string tmpFileName = $"{Guid.NewGuid()}{fileExtension}";

        // Use OneDrive path format
        string requestUrl = $"{path}root:/{tmpFileName}:/content";

        var requestContent = new StreamContent(content);
        requestContent.Headers.ContentType = new MediaTypeHeaderValue(contentType);

        Console.WriteLine($"Uploading file to {requestUrl}...");
        var response = await httpClient.PutAsync(requestUrl, requestContent);

        if (response.IsSuccessStatusCode)
        {
            var responseBody = await response.Content.ReadAsStringAsync();
            dynamic file = JsonConvert.DeserializeObject(responseBody)!;
            Console.WriteLine($"File uploaded successfully with ID: {file?.id}");
            return file?.id;
        }
        else
        {
            var message = await response.Content.ReadAsStringAsync();
            throw new Exception($"Upload file failed with status {response.StatusCode} and message {message}");
        }
    }

    // For downloading and deleting, the methods work similarly with OneDrive paths
    public async Task<byte[]> DownloadFileAsync(string path, string fileId, string format)
    {
        var httpClient = await CreateAuthorizedHttpClient(["Files.ReadWrite"]);
        var requestUrl = $"{path}items/{fileId}/content?format={format}";

        Console.WriteLine($"Downloading converted file from {requestUrl}...");
        var response = await httpClient.GetAsync(requestUrl);

        if (response.IsSuccessStatusCode)
        {
            var fileContent = await response.Content.ReadAsByteArrayAsync();
            Console.WriteLine($"Downloaded {fileContent.Length} bytes");
            return fileContent;
        }
        else
        {
            var message = await response.Content.ReadAsStringAsync();
            throw new Exception($"Download of converted file failed with status {response.StatusCode} and message {message}");
        }
    }

    public async Task DeleteFileAsync(string path, string fileId)
    {
        var httpClient = await CreateAuthorizedHttpClient(["Files.ReadWrite"]);
        var requestUrl = $"{path}{fileId}";

        Console.WriteLine($"Deleting file with ID {fileId}...");
        var response = await httpClient.DeleteAsync(requestUrl);

        if (response.IsSuccessStatusCode)
        {
            Console.WriteLine("File deleted successfully");
        }
        else
        {
            var message = await response.Content.ReadAsStringAsync();
            throw new Exception($"Delete file failed with status {response.StatusCode} and message {message}");
        }
    }
}
