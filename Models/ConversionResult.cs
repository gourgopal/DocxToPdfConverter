namespace DocxToPdfConverter.Models;
public class ConversionResult
{
    public bool Success { get; set; }
    public string OutputPath { get; set; } = string.Empty;
    public TimeSpan ConversionTime { get; set; }
    public string ErrorMessage { get; set; } = string.Empty;
}
