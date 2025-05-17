using DocxToPdfConverter.Models;
using System.Diagnostics;

namespace DocxToPdfConverter.Services;

public class GraphDocumentConversionService(IFileService fileService, ConversionOptions options) : IDocumentConversionService
{
    private readonly IFileService _fileService = fileService;
    private readonly ConversionOptions _options = options;

    public async Task<ConversionResult> ConvertDocumentAsync(string filePath, string outputPath)
    {
        var result = new ConversionResult();
        var stopwatch = Stopwatch.StartNew();

        try
        {
            Console.WriteLine($"{DateTime.Now} Starting conversion of {filePath} to {outputPath}");
            // Use OneDrive endpoint instead of SharePoint
            var path = $"{_options.GraphEndpoint}me/drive/";

            using var fileStream = File.OpenRead(filePath);
            var contentType = GetContentType(filePath);

            Console.WriteLine($"{DateTime.Now} Uploading document to Microsoft Graph...");
            var fileId = await _fileService.UploadFileAsync(path, fileStream, contentType);

            Console.WriteLine($"Converting document to PDF... {DateTime.Now}");
            var pdf = await _fileService.DownloadFileAsync(path, fileId, "pdf");

            Console.WriteLine($"Deleting temporary document from Microsoft Graph... {DateTime.Now}");
            await _fileService.DeleteFileAsync(path, fileId);

            Console.WriteLine($"{DateTime.Now} Saving PDF to {outputPath}...");
            File.WriteAllBytes(outputPath, pdf);

            result.Success = true;
            result.OutputPath = outputPath;
        }
        catch (Exception ex)
        {
            result.Success = false;
            result.ErrorMessage = ex.Message;
            Console.WriteLine($"{DateTime.Now} Error during conversion: {ex.Message}");
        }
        finally
        {
            stopwatch.Stop();
            result.ConversionTime = stopwatch.Elapsed;
        }

        return result;
    }

    private static string GetContentType(string filePath)
    {
        var extension = Path.GetExtension(filePath).ToLowerInvariant();
        return extension switch
        {
            ".docx" => "application/vnd.openxmlformats-officedocument.wordprocessingml.document",
            ".doc" => "application/msword",
            _ => throw new NotSupportedException($"File extension {extension} is not supported")
        };
    }
}
