namespace CsvTextEditor
{
    using System.Threading.Tasks;
    using Catel.IoC;
    using Catel.MVVM;
    using Orc.CsvTextEditor;
    using Orc.ProjectManagement;

    public class EditFindReplaceCommandContainer : EditProjectCommandContainerBase
    {
        public EditFindReplaceCommandContainer(ICommandManager commandManager, IProjectManager projectManager, ICsvTextEditorInstanceProvider csvTextEditorInstanceProvider)
            : base(Commands.Edit.FindReplace, commandManager, projectManager, csvTextEditorInstanceProvider)
        {
        }

        public override async Task ExecuteAsync(object parameter)
        {
            await CsvTextEditorInstance.ShowToolAsync<FindReplaceTool>();
        }
    }
}
