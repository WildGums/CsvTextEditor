namespace CsvTextEditor
{
    using Catel.IoC;
    using Catel.MVVM;
    using Orc.ProjectManagement;

    public class EditRemoveDuplicateLinesCommandContainer : EditProjectCommandContainerBase
    {
        #region Constructors
        public EditRemoveDuplicateLinesCommandContainer(ICommandManager commandManager, IProjectManager projectManager, IServiceLocator serviceLocator)
            : base(Commands.Edit.RemoveDuplicateLines, commandManager, projectManager, serviceLocator)
        {
        }

        #endregion

        #region Methods

        protected override void Execute(object parameter)
        {
            CsvTextEditorService.RemoveDuplicateLines();

            base.Execute(parameter);
        }
        #endregion
    }
}