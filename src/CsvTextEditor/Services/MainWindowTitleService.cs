namespace CsvTextEditor.Services
{
    using System;
    using Catel;
    using Catel.Reflection;
    using Orc.ProjectManagement;

    public class MainWindowTitleService : IMainWindowTitleService
    {
        private readonly string _defaulTitle;
        private readonly IProjectManager _projectManager;
        private readonly ShellActivatedActionQueue _shellActivatedActionQueue;

        public MainWindowTitleService(IProjectManager projectManager)
        {
            ArgumentNullException.ThrowIfNull(projectManager);

            _projectManager = projectManager;

            _shellActivatedActionQueue = new ShellActivatedActionQueue();

            _defaulTitle = AssemblyHelper.GetEntryAssembly().Title();
        }

        public void UpdateTitle()
        {
            _shellActivatedActionQueue.EnqueueAction(() =>
            {
                var project = _projectManager.ActiveProject;
                var app = System.Windows.Application.Current;
                var title = _defaulTitle;

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
