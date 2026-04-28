namespace CsvTextEditor.Services
{
    using Catel.Reflection;
    using Catel.Services;
    using Orc.ProjectManagement;
    using Orchestra;

    public class MainWindowTitleService : IMainWindowTitleService
    {
        private readonly string _defaultTitle;
        private readonly IProjectManager _projectManager;
        private readonly ShellActivatedActionQueue _shellActivatedActionQueue;

        public MainWindowTitleService(IProjectManager projectManager, 
            IDispatcherService dispatcherService, IMainWindowService mainWindowService)
        {
            _projectManager = projectManager;

            _shellActivatedActionQueue = new ShellActivatedActionQueue(dispatcherService, mainWindowService);

            _defaultTitle = AssemblyHelper.GetEntryAssembly().Title();
        }

        public void UpdateTitle()
        {
            _shellActivatedActionQueue.EnqueueAction(() =>
            {
                var project = _projectManager.ActiveProject;
                var app = System.Windows.Application.Current;
                var title = _defaultTitle;

                if (project is not null)
                {
                    title += $" - {project.Title}";

                    if (project.IsDirty)
                    {
                        title += " *";
                    }
                }

                app.MainWindow.SetCurrentValue(System.Windows.Window.TitleProperty, title);
            });
        }
    }
}
