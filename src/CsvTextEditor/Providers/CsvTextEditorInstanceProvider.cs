namespace CsvTextEditor
{
    using Models;
    using Orc.CsvTextEditor;

    public class CsvTextEditorInstanceProvider : ICsvTextEditorInstanceProvider
    {
        private readonly ICsvTextEditorInstanceManager _csvTextEditorInstanceManager;

        public CsvTextEditorInstanceProvider(ICsvTextEditorInstanceManager csvTextEditorInstanceManager)
        {
            _csvTextEditorInstanceManager = csvTextEditorInstanceManager;
        }

        public ICsvTextEditorInstance GetInstance(Project project)
        {
            return _csvTextEditorInstanceManager.GetInstance(project.EditorId);
        }
    }
}
