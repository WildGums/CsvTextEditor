namespace CsvTextEditor.ProjectManagement
{
    using System;
    using System.Diagnostics;
    using System.Threading.Tasks;
    using System.Windows.Threading;
    using Catel;
    using Catel.Configuration;
    using Catel.IoC;
    using Catel.Threading;
    using Models;
    using Orc.CsvTextEditor;
    using Orc.ProjectManagement;
    using Services;

    public class CsvTextEditorIsDirtyProjectWatcher : ProjectWatcherBase
    {
        #region Fields
        private readonly IMainWindowTitleService _mainWindowTitleService;
        private readonly ISaveProjectChangesService _saveProjectChangesService;
        private readonly IConfigurationService _configurationService;
        private readonly ICsvTextEditorInstanceProvider _csvTextEditorInstanceProvider;
        private readonly IProjectManager _projectManager;
        private ICsvTextEditorInstance _csvTextEditorInstance;
        private readonly DispatcherTimer _autoSaveTimer;
        #endregion

        #region Constructors
        public CsvTextEditorIsDirtyProjectWatcher(IProjectManager projectManager, IMainWindowTitleService mainWindowTitleService,
            ISaveProjectChangesService saveProjectChangesService, IConfigurationService configurationService, 
            ICsvTextEditorInstanceProvider csvTextEditorInstanceProvider)
            : base(projectManager)
        {
            ArgumentNullException.ThrowIfNull(mainWindowTitleService);
            ArgumentNullException.ThrowIfNull(saveProjectChangesService);
            ArgumentNullException.ThrowIfNull(configurationService);
            ArgumentNullException.ThrowIfNull(projectManager);

            _projectManager = projectManager;
            _mainWindowTitleService = mainWindowTitleService;
            _saveProjectChangesService = saveProjectChangesService;
            _configurationService = configurationService;
            _csvTextEditorInstanceProvider = csvTextEditorInstanceProvider;
            _autoSaveTimer = new DispatcherTimer();
            _autoSaveTimer.Tick += AutoSaveIfNeeded;
            _autoSaveTimer.Interval = configurationService.GetRoamingValue(Configuration.AutoSaveInterval, Configuration.AutoSaveIntervalDefaultValue);
            _autoSaveTimer.Start();
        }
        #endregion

        #region Methods
        protected override Task OnActivatedAsync(IProject oldProject, IProject newProject)
        {
            if (oldProject is not null && _csvTextEditorInstance is not null)
            {
                _csvTextEditorInstance.TextChanged -= CsvTextEditorInstanceOnTextChanged;
            }

            if (newProject is null)
            {
                return base.OnActivatedAsync(oldProject, newProject);
            }

            _csvTextEditorInstance = _csvTextEditorInstanceProvider.GetInstance((Project)newProject);

            if (_csvTextEditorInstance is not null)
            {
                _csvTextEditorInstance.TextChanged += CsvTextEditorInstanceOnTextChanged;
            }

            return base.OnActivatedAsync(oldProject, newProject);
        }

        private void AutoSaveIfNeeded(object sender, EventArgs e)
        {

            if (_csvTextEditorInstance is null)
            {
                return;
            }

            if (_csvTextEditorInstance.IsDirty &&
                _configurationService.GetRoamingValue<bool>(Configuration.AutoSaveEditor))
            {
                var project = (Project)ProjectManager.ActiveProject;
                _projectManager.SaveAsync(project.Location).ConfigureAwait(false);

            }
        }

        private void CsvTextEditorInstanceOnTextChanged(object sender, EventArgs e)
        {
            var project = (Project) ProjectManager.ActiveProject;

            if (project is not null)
            {
                project.SetIsDirty(_csvTextEditorInstance.IsDirty);
                project.Text = _csvTextEditorInstance.GetText();
            }

            _mainWindowTitleService.UpdateTitle();
        }

        protected override Task OnSavedAsync(IProject project)
        {
            var csvProject = (Project) ProjectManager.ActiveProject;
            csvProject?.SetIsDirty(_csvTextEditorInstance.IsDirty);
            _mainWindowTitleService.UpdateTitle();

            return base.OnSavedAsync(project);
        }

        protected override async Task OnClosingAsync(ProjectCancelEventArgs e)
        {
            if (e.Cancel)
            {
                return;
            }

            if (!await _saveProjectChangesService.EnsureChangesSavedAsync((Project)e.Project, SaveChangesReason.Closing))
            {
                e.Cancel = true;
            }
        }
        #endregion
    }
}
