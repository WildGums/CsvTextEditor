namespace CsvTextEditor
{
    using System;
    using System.Windows;
    using System.Windows.Media;
    using Catel.MVVM;
    using Catel.Services;
    using Orc.CsvTextEditor;
    using Orc.ProjectManagement;

    public class EditQuoteColumnCommandContainer : EditProjectCommandContainerBase
    {
        public EditQuoteColumnCommandContainer(ICommandManager commandManager, IProjectManager projectManager,
            ICsvTextEditorInstanceProvider csvTextEditorInstanceProvider, IServiceProvider serviceProvider,
            IDispatcherService dispatcherService)
            : base(Commands.Edit.QuoteColumn, commandManager, projectManager, csvTextEditorInstanceProvider, serviceProvider, dispatcherService)
        {
        }

        public override void Execute(object? parameter)
        {
            var editor = CsvTextEditorInstance?.GetEditor() as DependencyObject;
            if (editor is null)
            {
                return;
            }

            DependencyObject? current = editor;
            while (current is not null)
            {
                if (current is CsvTextEditorControl csvControl)
                {
                    CsvTextEditorControl.QuoteColumn.Execute(null, csvControl);
                    break;
                }

                current = VisualTreeHelper.GetParent(current);
            }

            base.Execute(parameter);
        }
    }
}
