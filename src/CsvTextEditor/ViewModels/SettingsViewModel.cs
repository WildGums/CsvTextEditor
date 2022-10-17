namespace CsvTextEditor.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Catel.Configuration;
    using Catel.MVVM;
    using Catel.Services;
    using Orc.Squirrel;
    using Orchestra.Services;

    public class SettingsViewModel : ViewModelBase
    {
        private readonly IConfigurationService _configurationService;
        private readonly IManageAppDataService _manageAppDataService;
        private readonly IUpdateService _updateService;
        private readonly IOpenFileService _openFileService;
   
        public SettingsViewModel(IConfigurationService configurationService, IManageAppDataService manageAppDataService, IUpdateService updateService, IOpenFileService openFileService)
        {
            ArgumentNullException.ThrowIfNull(configurationService);
            ArgumentNullException.ThrowIfNull(manageAppDataService);
            ArgumentNullException.ThrowIfNull(updateService);
            ArgumentNullException.ThrowIfNull(openFileService);

            _configurationService = configurationService;
            _manageAppDataService = manageAppDataService;
            _updateService = updateService;
            _openFileService = openFileService;

            PickEditor = new TaskCommand(PickEditorExecuteAsync);
            OpenApplicationDataDirectory = new Command(OnOpenApplicationDataDirectoryExecute);
            BackupUserData = new TaskCommand(OnBackupUserDataExecuteAsync);

            Title = "Settings";
        }

        public bool IsUpdateSystemAvailable { get; private set; }
        public bool CheckForUpdates { get; set; }
        public bool AutoSaveEditor { get; set; }
        public string CustomEditor { get; private set; }
        public List<UpdateChannel> AvailableUpdateChannels { get; private set; }
        public UpdateChannel UpdateChannel { get; set; }

        public Command OpenApplicationDataDirectory { get; private set; }

        private void OnOpenApplicationDataDirectoryExecute()
        {
            _manageAppDataService.OpenApplicationDataDirectory(Catel.IO.ApplicationDataTarget.UserRoaming);
        }

        public TaskCommand PickEditor { get; private set; }

        private async Task PickEditorExecuteAsync()
        {
            var result = await _openFileService.DetermineFileAsync(new DetermineOpenFileContext
            {
                Filter = "Program Files (*.exe)|*exe",
                IsMultiSelect = false
            });

            if (result.Result)
            {
                CustomEditor = result.FileName;
            }       
        }

        public TaskCommand BackupUserData { get; private set; }

        private async Task OnBackupUserDataExecuteAsync()
        {
            await _manageAppDataService.BackupUserDataAsync(Catel.IO.ApplicationDataTarget.UserRoaming);
        }

        protected override async Task InitializeAsync()
        {
            await base.InitializeAsync();

            IsUpdateSystemAvailable = _updateService.IsUpdateSystemAvailable;
            CheckForUpdates = await _updateService.GetCheckForUpdatesAsync();
            AvailableUpdateChannels = new List<UpdateChannel>(_updateService.AvailableChannels);
            UpdateChannel = await _updateService.GetCurrentChannelAsync();

            CustomEditor = _configurationService.GetRoamingValue<string>(Configuration.CustomEditor);
            AutoSaveEditor = _configurationService.GetRoamingValue(Configuration.AutoSaveEditor, Configuration.AutoSaveEditorDefaultValue);
        }

        protected override async Task<bool> SaveAsync()
        {
            await _updateService.SetCheckForUpdatesAsync(CheckForUpdates);
            await _updateService.SetCurrentChannelAsync(UpdateChannel);

            _configurationService.SetRoamingValue(Configuration.CustomEditor, CustomEditor);
            _configurationService.SetRoamingValue(Configuration.AutoSaveEditor, AutoSaveEditor);

            return await base.SaveAsync();
        }
    }
}
