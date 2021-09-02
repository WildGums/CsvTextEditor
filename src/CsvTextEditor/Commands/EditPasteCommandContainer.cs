namespace CsvTextEditor
{
    using System.Windows;
    using Catel.MVVM;
    using Orc.ProjectManagement;

    public class EditPasteCommandContainer : EditProjectCommandContainerBase
    {
        #region Constructors
        public EditPasteCommandContainer(ICommandManager commandManager, IProjectManager projectManager, ICsvTextEditorInstanceProvider csvTextEditorInstanceProvider)
            : base(Commands.Edit.Paste, commandManager, projectManager, csvTextEditorInstanceProvider)
        {
        }
        #endregion

        #region Methods
        protected override void Execute(object parameter)
        {
            if (Clipboard.ContainsText())
            {
                CsvTextEditorInstance?.Paste();
            }

            base.Execute(parameter);
        }
        #endregion
    }
}
