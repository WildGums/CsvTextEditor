namespace CsvTextEditor
{
    using System;
    using System.Threading.Tasks;
    using Catel.MVVM;
    using Models;
    using Orc.ProjectManagement;

    public abstract class ProjectCommandContainerBase : CommandContainerBase
    {
        protected readonly ICommandManager _commandManager;
        protected readonly IProjectManager _projectManager;

        protected ProjectCommandContainerBase(string commandName, ICommandManager commandManager, IProjectManager projectManager)
            : base(commandName, commandManager)
        {
            ArgumentNullException.ThrowIfNull(projectManager);

            _commandManager = commandManager;
            _projectManager = projectManager;

            _projectManager.ProjectActivatedAsync += OnProjectActivatedAsync;
        }

        private Task OnProjectActivatedAsync(object sender, ProjectUpdatedEventArgs e)
        {
            ProjectActivated((Project)e.OldProject, (Project)e.NewProject);

            _commandManager.InvalidateCommands();

            return Task.CompletedTask;
        }

        protected virtual void ProjectActivated(Project oldProject, Project newProject)
        {
        }

        public override bool CanExecute(object parameter)
        {
            if (_projectManager.ActiveProject is null)
            {
                return false;
            }

            return base.CanExecute(parameter);
        }
    }
}
