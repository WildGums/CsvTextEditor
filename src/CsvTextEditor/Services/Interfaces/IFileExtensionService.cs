namespace CsvTextEditor.Services
{
    public interface IFileExtensionService
    {
        string GetRegisteredTool(string extension);
    }
}