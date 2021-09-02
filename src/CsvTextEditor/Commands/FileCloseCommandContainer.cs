namespace CsvTextEditor
{
    using System.Threading.Tasks;
    using Catel.MVVM;
    using Orc.ProjectManagement;

    public class FileCloseCommandContainer : ProjectCommandContainerBase
    {
        #region Constructors
        public FileCloseCommandContainer(ICommandManager commandManager, IProjectManager projectManager)
            : base(Commands.File.Close, commandManager, projectManager)
        {
        }
        #endregion

        #region Methods
        protected override async Task ExecuteAsync(object parameter)
        {
            var activeProject = _projectManager.ActiveProject;
            if (activeProject is null)
            {
                return;
            }

            await _projectManager.CloseAsync(activeProject);

            await base.ExecuteAsync(parameter);
        }
        #endregion
    }
}
