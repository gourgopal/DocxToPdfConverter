using DocxToPdfConverter.Models;

namespace DocxToPdfConverter.Services;

public interface IDocumentConversionService
{
    Task<ConversionResult> ConvertDocumentAsync(string filePath, string outputPath);
}
