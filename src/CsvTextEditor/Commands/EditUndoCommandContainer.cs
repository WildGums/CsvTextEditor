// --------------------------------------------------------------------------------------------------------------------
namespace CsvTextEditor
{
    using Catel.IoC;
    using Catel.MVVM;
    using Orc.ProjectManagement;

    public class EditUndoCommandContainer : EditProjectCommandContainerBase
    {
        public EditUndoCommandContainer(ICommandManager commandManager, IProjectManager projectManager, ICsvTextEditorInstanceProvider csvTextEditorInstanceProvider)
            : base(Commands.Edit.Undo, commandManager, projectManager, csvTextEditorInstanceProvider)
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
