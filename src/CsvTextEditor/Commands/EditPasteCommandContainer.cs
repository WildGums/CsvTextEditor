namespace CsvTextEditor
{
    using System;
    using System.Windows;
    using Catel.MVVM;
    using Catel.Services;
    using Orc.ProjectManagement;

    public class EditPasteCommandContainer : EditProjectCommandContainerBase
    {
        public EditPasteCommandContainer(ICommandManager commandManager, IProjectManager projectManager, 
            ICsvTextEditorInstanceProvider csvTextEditorInstanceProvider, IServiceProvider serviceProvider, IDispatcherService dispatcherService)
            : base(Commands.Edit.Paste, commandManager, projectManager, csvTextEditorInstanceProvider, serviceProvider, dispatcherService)
        {
        }

        public override void Execute(object parameter)
        {
            if (Clipboard.ContainsText())
            {
                CsvTextEditorInstance?.Paste();
            }

            base.Execute(parameter);
        }
    }
}
