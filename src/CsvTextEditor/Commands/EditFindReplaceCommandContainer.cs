namespace CsvTextEditor
{
    using System;
    using System.Threading.Tasks;
    using Catel.MVVM;
    using Catel.Services;
    using Orc.CsvTextEditor;
    using Orc.ProjectManagement;

    public class EditFindReplaceCommandContainer : EditProjectCommandContainerBase
    {
        public EditFindReplaceCommandContainer(ICommandManager commandManager, IProjectManager projectManager, 
            ICsvTextEditorInstanceProvider csvTextEditorInstanceProvider, IServiceProvider serviceProvider, IDispatcherService dispatcherService)
            : base(Commands.Edit.FindReplace, commandManager, projectManager, csvTextEditorInstanceProvider, serviceProvider, dispatcherService)
        {
        }

        public override async Task ExecuteAsync(object parameter)
        {
            await CsvTextEditorInstance.ShowToolAsync<FindReplaceTool>();
        }
    }
}
