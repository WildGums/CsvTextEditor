namespace CsvTextEditor
{
    using Catel.IoC;
    using Catel.MVVM;
    using Orc.ProjectManagement;

    public class EditRedoCommandContainer : EditProjectCommandContainerBase
    {
        #region Constructors
        public EditRedoCommandContainer(ICommandManager commandManager, IProjectManager projectManager, ICsvTextEditorInstanceProvider csvTextEditorInstanceProvider)
            : base(Commands.Edit.Redo, commandManager, projectManager, csvTextEditorInstanceProvider)
        {
        }
        #endregion

        #region Methods
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
        #endregion
    }
}
