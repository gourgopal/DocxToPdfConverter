namespace DocxToPdfConverter.Services;
public interface IConfigurationService
{
    T GetConfiguration<T>(string sectionName) where T : class, new();
}
