namespace CsvTextEditor
{
    using System;
    using System.Threading.Tasks;
    using Catel.MVVM;
    using Catel.Services;
    using Orc.ProjectManagement;

    public class FileCloseCommandContainer : ProjectCommandContainerBase
    {
        public FileCloseCommandContainer(ICommandManager commandManager, IProjectManager projectManager, 
            IServiceProvider serviceProvider)
            : base(Commands.File.Close, commandManager, projectManager, serviceProvider)
        {
        }

        public override async Task ExecuteAsync(object parameter)
        {
            var activeProject = _projectManager.ActiveProject;
            if (activeProject is null)
            {
                return;
            }

            await _projectManager.CloseAsync(activeProject);

            await base.ExecuteAsync(parameter);
        }
    }
}
