namespace CsvTextEditor
{
    using System;
    using Catel.IoC;
    using Catel.MVVM;
    using Catel.Services;
    using Orc.ProjectManagement;

    public class EditUndoCommandContainer : EditProjectCommandContainerBase
    {
        public EditUndoCommandContainer(ICommandManager commandManager, IProjectManager projectManager, 
            ICsvTextEditorInstanceProvider csvTextEditorInstanceProvider, IServiceProvider serviceProvider, IDispatcherService dispatcherService)
            : base(Commands.Edit.Undo, commandManager, projectManager, csvTextEditorInstanceProvider, serviceProvider, dispatcherService)
        {
        }

        public override bool CanExecute(object parameter)
        {
            if (!base.CanExecute(parameter))
            {
                return false;
            }

            return CsvTextEditorInstance?.CanUndo ?? false;
        }

        public override void Execute(object parameter)
        {
            CsvTextEditorInstance.Undo();

            base.Execute(parameter);
        }
    }
}
