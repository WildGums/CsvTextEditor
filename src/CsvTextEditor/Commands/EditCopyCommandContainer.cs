namespace CsvTextEditor
{
    using System;
    using Catel.MVVM;
    using Catel.Services;
    using Orc.ProjectManagement;

    public class EditCopyCommandContainer : EditProjectCommandContainerBase
    {
        public EditCopyCommandContainer(ICommandManager commandManager, IProjectManager projectManager, 
            ICsvTextEditorInstanceProvider csvTextEditorInstanceProvider, IServiceProvider serviceProvider,
            IDispatcherService dispatcherService)
            : base(Commands.Edit.Copy, commandManager, projectManager, csvTextEditorInstanceProvider, serviceProvider, dispatcherService)
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
            CsvTextEditorInstance.Copy();

            base.Execute(parameter);
        }
    }
}
