namespace CsvTextEditor
{
    using System.Threading.Tasks;
    using Catel;
    using Catel.MVVM;
    using Catel.Threading;
    using Models;
    using Orc.ProjectManagement;

    public abstract class ProjectCommandContainerBase : CommandContainerBase
    {
        #region Fields
        protected readonly ICommandManager _commandManager;

        protected readonly IProjectManager _projectManager;
        #endregion

        #region Constructors
        protected ProjectCommandContainerBase(string commandName, ICommandManager commandManager, IProjectManager projectManager)
            : base(commandName, commandManager)
        {
            Argument.IsNotNull(() => projectManager);

            _commandManager = commandManager;
            _projectManager = projectManager;

            _projectManager.ProjectActivatedAsync += OnProjectActivatedAsync;
        }
        #endregion

        #region Methods
        private Task OnProjectActivatedAsync(object sender, ProjectUpdatedEventArgs e)
        {
            ProjectActivated((Project)e.OldProject, (Project)e.NewProject);

            _commandManager.InvalidateCommands();

            return TaskHelper.Completed;
        }

        protected virtual void ProjectActivated(Project oldProject, Project newProject)
        {
        }

        protected override bool CanExecute(object parameter)
        {
            if (_projectManager.ActiveProject is null)
            {
                return false;
            }

            return base.CanExecute(parameter);
        }
        #endregion
    }
}
