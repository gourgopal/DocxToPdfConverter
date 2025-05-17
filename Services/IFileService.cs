namespace DocxToPdfConverter.Services;

public interface IFileService
{
    Task<string> UploadFileAsync(string path, Stream content, string contentType);
    Task<byte[]> DownloadFileAsync(string path, string fileId, string format);
    Task DeleteFileAsync(string path, string fileId);
}
