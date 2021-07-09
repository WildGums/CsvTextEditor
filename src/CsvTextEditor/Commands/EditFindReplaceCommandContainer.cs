namespace CsvTextEditor
{
    using Catel.IoC;
    using Catel.MVVM;
    using Orc.CsvTextEditor;
    using Orc.ProjectManagement;

    public class EditFindReplaceCommandContainer : EditProjectCommandContainerBase
    {
        #region Constructors
        public EditFindReplaceCommandContainer(ICommandManager commandManager, IProjectManager projectManager, ICsvTextEditorInstanceProvider csvTextEditorInstanceProvider)
            : base(Commands.Edit.FindReplace, commandManager, projectManager, csvTextEditorInstanceProvider)
        {
        }
        #endregion

        #region Methods
        protected override void Execute(object parameter)
        {
            CsvTextEditorInstance.ShowTool<FindReplaceTool>();
        }
        #endregion
    }
}
