namespace CsvTextEditor
{
    using System.Threading.Tasks;
    using Catel.MVVM;
    using Models;
    using Orc.ProjectManagement;

    public class FileSaveCommandContainer : ProjectCommandContainerBase
    {
        #region Constructors
        public FileSaveCommandContainer(ICommandManager commandManager, IProjectManager projectManager)
            : base(Commands.File.Save, commandManager, projectManager)
        {
        }
        #endregion

        #region Methods
        protected override async Task ExecuteAsync(object parameter)
        {
            await base.ExecuteAsync(parameter);

            if (_projectManager.ActiveProject is Project project)
            {
                await _projectManager.SaveAsync(project);
            }
        }
        #endregion
    }
}
