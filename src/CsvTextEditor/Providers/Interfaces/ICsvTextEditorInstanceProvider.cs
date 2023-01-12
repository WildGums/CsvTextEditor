namespace CsvTextEditor
{
    using Models;
    using Orc.CsvTextEditor;

    public interface ICsvTextEditorInstanceProvider
    {
        #region Methods
        ICsvTextEditorInstance GetInstance(Project project);
        #endregion
    }
}
