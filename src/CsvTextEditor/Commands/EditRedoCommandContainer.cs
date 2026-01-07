namespace CsvTextEditor
{
    using System;
    using Catel.IoC;
    using Catel.MVVM;
    using Catel.Services;
    using Orc.ProjectManagement;

    public class EditRedoCommandContainer : EditProjectCommandContainerBase
    {
        public EditRedoCommandContainer(ICommandManager commandManager, IProjectManager projectManager, 
            ICsvTextEditorInstanceProvider csvTextEditorInstanceProvider, IServiceProvider serviceProvider, IDispatcherService dispatcherService)
            : base(Commands.Edit.Redo, commandManager, projectManager, csvTextEditorInstanceProvider, serviceProvider, dispatcherService)
        {
        }

        public override bool CanExecute(object parameter)
        {
            if (!base.CanExecute(parameter))
            {
                return false;
            }

            return CsvTextEditorInstance?.CanRedo ?? false;
        }

        public override void Execute(object parameter)
        {
            CsvTextEditorInstance.Redo();

            base.Execute(parameter);
        }
    }
}
