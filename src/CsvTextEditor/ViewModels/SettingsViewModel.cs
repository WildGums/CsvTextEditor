// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SettingsViewModel.cs" company="WildGums">
//   Copyright (c) 2008 - 2017 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace CsvTextEditor.ViewModels
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Catel;
    using Catel.Configuration;
    using Catel.IoC;
    using Catel.MVVM;
    using Catel.Services;
    using Orc.Squirrel;
    using Services;

    public class SettingsViewModel : ViewModelBase
    {
        #region Fields
        private readonly IConfigurationService _configurationService;
        private readonly IManageUserDataService _manageUserDataService;
        private readonly IUpdateService _updateService;
        #endregion

        #region Constructors
        public SettingsViewModel(IConfigurationService configurationService, IManageUserDataService manageUserDataService, IUpdateService updateService)
        {
            Argument.IsNotNull(() => configurationService);
            Argument.IsNotNull(() => manageUserDataService);
            Argument.IsNotNull(() => updateService);

            _configurationService = configurationService;
            _manageUserDataService = manageUserDataService;
            _updateService = updateService;

            PickEditor = new TaskCommand(PickEditorExecuteAsync);
            OpenApplicationDataDirectory = new Command(OnOpenApplicationDataDirectoryExecute);
            BackupUserData = new TaskCommand(OnBackupUserDataExecuteAsync);

            Title = "Settings";
        }
        #endregion

        #region Properties
        public bool IsUpdateSystemAvailable { get; private set; }
        public bool CheckForUpdates { get; set; }
        public string CustomEditor { get; private set; }
        public List<UpdateChannel> AvailableUpdateChannels { get; private set; }
        public UpdateChannel UpdateChannel { get; set; }
        #endregion

        #region Commands
        public Command OpenApplicationDataDirectory { get; private set; }

        private void OnOpenApplicationDataDirectoryExecute()
        {
            _manageUserDataService.OpenApplicationDataDirectory();
        }

        public TaskCommand PickEditor { get; private set; }

        private async Task PickEditorExecuteAsync()
        {
            var resolver = ServiceLocator.Default.GetDependencyResolver();
            var openFileService = resolver.Resolve<IOpenFileService>();

            openFileService.Filter = "Program Files (*.exe)|*exe";
            openFileService.IsMultiSelect = false;

            var result = await openFileService.DetermineFileAsync();

            CustomEditor = openFileService.FileName;
        }

        public TaskCommand BackupUserData { get; private set; }

        private async Task OnBackupUserDataExecuteAsync()
        {
            await _manageUserDataService.BackupUserDataAsync();
        }
        #endregion

        #region Methods
        protected override async Task InitializeAsync()
        {
            await base.InitializeAsync();

            IsUpdateSystemAvailable = _updateService.IsUpdateSystemAvailable;
            CheckForUpdates = _updateService.CheckForUpdates;
            AvailableUpdateChannels = new List<UpdateChannel>(_updateService.AvailableChannels);
            UpdateChannel = _updateService.CurrentChannel;
            CustomEditor = _configurationService.GetLocalValue<string>(Configuration.CustomEditor);

        }

        protected override async Task<bool> SaveAsync()
        {
            _updateService.CheckForUpdates = CheckForUpdates;
            _updateService.CurrentChannel = UpdateChannel;


            _configurationService.SetLocalValue(Configuration.CustomEditor, CustomEditor);

            return await base.SaveAsync();
        }
        #endregion
    }
}
