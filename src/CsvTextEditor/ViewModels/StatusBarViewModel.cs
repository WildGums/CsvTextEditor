namespace CsvTextEditor.ViewModels
{
    using System.Threading.Tasks;
    using Catel.MVVM;
    using Orc.CsvTextEditor;
    using Orc.ProjectManagement;
    using Orchestra;
    using Orc.Squirrel;
    using System;
    using Catel.Configuration;
    using CsvTextEditor.Models;

    public class StatusBarViewModel : ViewModelBase
    {
        private readonly IProjectManager _projectManager;
        private ICsvTextEditorInstance _csvTextEditorInstance;
        private readonly IConfigurationService _configurationService;
        private readonly IUpdateService _updateService;
        private readonly ICsvTextEditorInstanceProvider _csvTextEditorInstanceProvider;

        public StatusBarViewModel(IProjectManager projectManager, IConfigurationService configurationService,
            IUpdateService updateService, ICsvTextEditorInstanceProvider csvTextEditorInstanceProvider)
        {
            ArgumentNullException.ThrowIfNull(projectManager);
            ArgumentNullException.ThrowIfNull(configurationService);
            ArgumentNullException.ThrowIfNull(updateService);
            ArgumentNullException.ThrowIfNull(csvTextEditorInstanceProvider);

            _projectManager = projectManager;
            _configurationService = configurationService;
            _updateService = updateService;
            _csvTextEditorInstanceProvider = csvTextEditorInstanceProvider;
        }

        public string ReceivingAutomaticUpdates { get; private set; }
        public bool IsUpdatedInstalled { get; private set; }
        public string Version { get; private set; }
        public int Column { get; private set; }
        public string Heading { get; private set; }
        public int Line { get; private set; }

        protected override async Task InitializeAsync()
        {
            await base.InitializeAsync();

            _configurationService.ConfigurationChanged += OnConfigurationChanged;
            _updateService.UpdateInstalled += OnUpdateInstalled;
            _projectManager.ProjectActivatedAsync += OnProjectActivatedAsync;

            IsUpdatedInstalled = _updateService.IsUpdatedInstalled;
            Version = VersionHelper.GetCurrentVersion();

            await UpdateAutoUpdateInfoAsync();
        }

        protected override async Task CloseAsync()
        {
            _configurationService.ConfigurationChanged -= OnConfigurationChanged;
            _updateService.UpdateInstalled -= OnUpdateInstalled;
            _projectManager.ProjectActivatedAsync -= OnProjectActivatedAsync;

            await base.CloseAsync();
        }

        private async void OnConfigurationChanged(object sender, ConfigurationChangedEventArgs e)
        {
            if (e.Key.Contains("Updates"))
            {
                await UpdateAutoUpdateInfoAsync();
            }
        }

        private void OnUpdateInstalled(object sender, EventArgs e)
        {
            IsUpdatedInstalled = _updateService.IsUpdatedInstalled;
        }

        private async Task UpdateAutoUpdateInfoAsync()
        {
            string updateInfo = string.Empty;

            var checkForUpdates = await _updateService.GetCheckForUpdatesAsync();
            if (!_updateService.IsUpdateSystemAvailable || !checkForUpdates)
            {
                updateInfo = "Automatic updates are disabled";
            }
            else
            {
                var channel = await _updateService.GetCurrentChannelAsync();
                updateInfo = string.Format("Automatic updates are enabled for {0} versions", channel.Name.ToLower());
            }

            ReceivingAutomaticUpdates = updateInfo;
        }

        private Task OnProjectActivatedAsync(object sender, ProjectUpdatedEventArgs args)
        {
            if (_csvTextEditorInstance is not null && args.OldProject is not null)
            {
                _csvTextEditorInstance.CaretTextLocationChanged -= OnCaretTextLocationChanged;
            }

            var activeProject = args.NewProject;
            if (activeProject is null)
            {
                return Task.CompletedTask;
            }

            _csvTextEditorInstance = _csvTextEditorInstanceProvider.GetInstance((Project)args.NewProject);

            _csvTextEditorInstance.CaretTextLocationChanged += OnCaretTextLocationChanged;

            return Task.CompletedTask;
        }

        private void OnCaretTextLocationChanged(object sender, CaretTextLocationChangedEventArgs args)
        {
            Column = args.Location.Column.Index;
            Line = args.Location.Line.Index + 1;
            Heading = HeadingForLocation(args.Location);
        }

        private string HeadingForLocation(Location location)
        {
            var allText = _csvTextEditorInstance.GetText();
            var indexOfNewLine = allText.IndexOf(Symbols.NewLineEnd);

            if (indexOfNewLine != -1)
            {
                var firstLine = allText.Substring(0, indexOfNewLine);
                var columnHeaders = firstLine.Split(Symbols.Comma);
                return columnHeaders[location.Column.Index].Trim();
            }

            return string.Empty;
        }
    }
}
