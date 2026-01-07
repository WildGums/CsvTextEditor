namespace CsvTextEditor
{
    using Models;
    using Orc.CsvTextEditor;

    public interface ICsvTextEditorInstanceProvider
    {
        ICsvTextEditorInstance GetInstance(Project project);
    }
}
