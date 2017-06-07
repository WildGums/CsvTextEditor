// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MainWindowTitleProjectWatcher.cs" company="WildGums">
//   Copyright (c) 2008 - 2017 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace CsvTextEditor.ProjectManagement
{
    using System.Threading.Tasks;
    using Catel;
    using Orc.ProjectManagement;
    using Services;

    public class MainWindowTitleProjectWatcher : ProjectWatcherBase
    {
        #region Fields
        private readonly IMainWindowTitleService _mainWindowTitleService;
        #endregion

        #region Constructors
        public MainWindowTitleProjectWatcher(IProjectManager projectManager, IMainWindowTitleService mainWindowTitleService)
            : base(projectManager)
        {
            Argument.IsNotNull(() => mainWindowTitleService);

            _mainWindowTitleService = mainWindowTitleService;
        }
        #endregion

        #region Methods
        protected override Task OnActivatedAsync(IProject oldProject, IProject newProject)
        {
            _mainWindowTitleService.UpdateTitle();

            return base.OnActivatedAsync(oldProject, newProject);
        }
        #endregion
    }
}