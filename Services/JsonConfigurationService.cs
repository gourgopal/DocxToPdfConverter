using Microsoft.Extensions.Configuration;

namespace DocxToPdfConverter.Services
{
    public class JsonConfigurationService(string configPath) : IConfigurationService
    {
        private readonly IConfiguration _configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile(configPath, optional: false, reloadOnChange: true)
                .Build();

        public T GetConfiguration<T>(string sectionName) where T : class, new()
        {
            var section = _configuration.GetSection(sectionName);
            var config = new T();
            section.Bind(config);
            return config;
        }
    }
}
