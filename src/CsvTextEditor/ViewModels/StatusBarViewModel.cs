// --------------------------------------------------------------------------------------------------------------------
// <copyright file="StatusBarViewModel.cs" company="WildGums">
//   Copyright (c) 2008 - 2017 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace CsvTextEditor.ViewModels
{
    using System.Threading.Tasks;
    using Catel;
    using Catel.IoC;
    using Catel.MVVM;
    using Catel.Threading;
    using Orc.CsvTextEditor;
    using Orc.ProjectManagement;
    using Orchestra;
    using Orc.Squirrel;
    using System;
    using Catel.Configuration;

    public class StatusBarViewModel : ViewModelBase
    {
        #region Fields
        private readonly IProjectManager _projectManager;
        private readonly IServiceLocator _serviceLocator;
        private ICsvTextEditorInstance _csvTextEditorInstance;
        private readonly IConfigurationService _configurationService;
        private readonly IUpdateService _updateService;
        #endregion

        #region Constructors
        public StatusBarViewModel(IProjectManager projectManager, IServiceLocator serviceLocator, IConfigurationService configurationService,
            IUpdateService updateService)
        {
            Argument.IsNotNull(() => projectManager);
            Argument.IsNotNull(() => serviceLocator);
            Argument.IsNotNull(() => configurationService);
            Argument.IsNotNull(() => updateService);

            _projectManager = projectManager;
            _serviceLocator = serviceLocator;
            _configurationService = configurationService;
            _updateService = updateService;
        }
        #endregion

        #region Properties
        public string ReceivingAutomaticUpdates { get; private set; }
        public bool IsUpdatedInstalled { get; private set; }
        public string Version { get; private set; }
        public int Column { get; private set; }
        public string Heading { get; private set; }
        public int Line { get; private set; }
        #endregion

        #region Methods
        protected override async Task InitializeAsync()
        {
            await base.InitializeAsync();

            _configurationService.ConfigurationChanged += OnConfigurationChanged;
            _updateService.UpdateInstalled += OnUpdateInstalled;
            _projectManager.ProjectActivatedAsync += OnProjectActivatedAsync;

            IsUpdatedInstalled = _updateService.IsUpdatedInstalled;
            Version = VersionHelper.GetCurrentVersion();

            UpdateAutoUpdateInfo();
        }

        protected override async Task CloseAsync()
        {
            _configurationService.ConfigurationChanged -= OnConfigurationChanged;
            _updateService.UpdateInstalled -= OnUpdateInstalled;
            _projectManager.ProjectActivatedAsync -= OnProjectActivatedAsync;

            await base.CloseAsync();
        }

        private void OnConfigurationChanged(object sender, ConfigurationChangedEventArgs e)
        {
            if (e.Key.Contains("Updates"))
            {
                UpdateAutoUpdateInfo();
            }
        }

        private void OnUpdateInstalled(object sender, EventArgs e)
        {
            IsUpdatedInstalled = _updateService.IsUpdatedInstalled;
        }

        private void UpdateAutoUpdateInfo()
        {
            string updateInfo = string.Empty;

            var checkForUpdates = _updateService.CheckForUpdates;
            if (!_updateService.IsUpdateSystemAvailable || !checkForUpdates)
            {
                updateInfo = "Automatic updates are disabled";
            }
            else
            {
                var channel = _updateService.CurrentChannel.Name;
                updateInfo = string.Format("Automatic updates are enabled for {0} versions", channel.ToLower());
            }

            ReceivingAutomaticUpdates = updateInfo;
        }

        private Task OnProjectActivatedAsync(object sender, ProjectUpdatedEventArgs args)
        {
            if (_csvTextEditorInstance != null)
            {
                _csvTextEditorInstance.CaretTextLocationChanged -= OnCaretTextLocationChanged;
            }

            var activeProject = args.NewProject;
            if (activeProject == null)
            {
                return TaskHelper.Completed;
            }

            _csvTextEditorInstance = _serviceLocator.ResolveType<ICsvTextEditorInstance>(args.NewProject);

            _csvTextEditorInstance.CaretTextLocationChanged += OnCaretTextLocationChanged;

            return TaskHelper.Completed;
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
            var firstLine = allText.Substring(0, allText.IndexOf(Symbols.NewLineEnd));
            var columnHeaders = firstLine.Split(Symbols.Comma);
            return columnHeaders[location.Column.Index].Trim();
        }
        #endregion
    }
}