namespace CsvTextEditor
{
    using System;
    using Catel.MVVM;
    using Catel.Services;
    using Orc.ProjectManagement;

    public class EditCutCommandContainer : EditProjectCommandContainerBase
    {
        public EditCutCommandContainer(ICommandManager commandManager, IProjectManager projectManager, 
            ICsvTextEditorInstanceProvider csvTextEditorInstanceProvider, IServiceProvider serviceProvider, IDispatcherService dispatcherService)
            : base(Commands.Edit.Cut, commandManager, projectManager, csvTextEditorInstanceProvider, serviceProvider, dispatcherService)
        {
        }

        public override bool CanExecute(object parameter)
        {
            if (!base.CanExecute(parameter))
            {
                return false;
            }

            return CsvTextEditorInstance?.HasSelection ?? false;
        }

        public override void Execute(object parameter)
        {
            CsvTextEditorInstance.Cut();

            base.Execute(parameter);
        }
    }
}
