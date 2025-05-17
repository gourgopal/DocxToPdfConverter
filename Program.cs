using DocxToPdfConverter.Models;
using DocxToPdfConverter.Services;
using Microsoft.Extensions.DependencyInjection;

namespace DocxToPdfConverter;
class Program
{
    static async Task Main(string[] args)
    {
        try
        {
            // Parse command line arguments
            if (args.Length < 2)
            {
                Console.WriteLine("Usage: DocxToPdfConverter <input-file> <output-file>");
                return;
            }

            var inputFile = args[0];
            var outputFile = args[1];

            // Validate input file
            if (!File.Exists(inputFile))
            {
                Console.WriteLine($"Input file not found: {inputFile}");
                return;
            }

            // Setup dependency injection
            var serviceProvider = ConfigureServices();

            // Get document conversion service
            var conversionService = serviceProvider.GetRequiredService<IDocumentConversionService>();

            Console.WriteLine($"Converting {inputFile} to {outputFile}...");

            // Perform conversion
            var result = await conversionService.ConvertDocumentAsync(inputFile, outputFile);

            // Display results
            if (result.Success)
            {
                Console.WriteLine($"Conversion successful. Output saved to: {result.OutputPath}");
                Console.WriteLine($"Conversion time: {result.ConversionTime.TotalSeconds:F2} seconds");
            }
            else
            {
                Console.WriteLine($"Conversion failed: {result.ErrorMessage}");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred: {ex.Message}");
        }
    }

    private static ServiceProvider ConfigureServices()
    {
        var configService = new JsonConfigurationService("appsettings.json");
        var authOptions = configService.GetConfiguration<AuthenticationOptions>("graph");
        var conversionOptions = configService.GetConfiguration<ConversionOptions>("pdf");

        var services = new ServiceCollection();

        // Authentication setup
        services.AddSingleton<IAuthenticationService>(provider =>
            new DelegatedAuthenticationService(
                authOptions.ClientId,
                ["Files.ReadWrite.All", "User.Read"] // Delegated permissions
            ));

        services.AddSingleton(conversionOptions);
        services.AddSingleton<IFileService, GraphFileService>();
        services.AddSingleton<IDocumentConversionService, GraphDocumentConversionService>();

        return services.BuildServiceProvider();
    }
}
