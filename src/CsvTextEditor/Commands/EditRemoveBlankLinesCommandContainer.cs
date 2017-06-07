namespace CsvTextEditor
{
    using Catel.IoC;
    using Catel.MVVM;
    using Orc.ProjectManagement;

    public class EditRemoveBlankLinesCommandContainer : EditProjectCommandContainerBase
    {
        #region Constructors
        public EditRemoveBlankLinesCommandContainer(ICommandManager commandManager, IProjectManager projectManager, IServiceLocator serviceLocator)
            : base(Commands.Edit.RemoveBlankLines, commandManager, projectManager, serviceLocator)
        {
        }

        #endregion

        #region Methods

        protected override void Execute(object parameter)
        {
            CsvTextEditorService.RemoveBlankLines();

            base.Execute(parameter);
        }
        #endregion
    }
}