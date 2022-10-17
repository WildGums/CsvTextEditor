namespace CsvTextEditor.ProjectManagement
{
    using System;
    using System.Threading.Tasks;
    using Catel;
    using Orc.ProjectManagement;
    using Services;

    public class MainWindowTitleProjectWatcher : ProjectWatcherBase
    {
        private readonly IMainWindowTitleService _mainWindowTitleService;

        public MainWindowTitleProjectWatcher(IProjectManager projectManager, IMainWindowTitleService mainWindowTitleService)
            : base(projectManager)
        {
            ArgumentNullException.ThrowIfNull(mainWindowTitleService);

            _mainWindowTitleService = mainWindowTitleService;
        }

        protected override Task OnActivatedAsync(IProject oldProject, IProject newProject)
        {
            _mainWindowTitleService.UpdateTitle();

            return base.OnActivatedAsync(oldProject, newProject);
        }
    }
}
