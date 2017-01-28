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
    using Catel.MVVM;
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

            OpenApplicationDataDirectory = new Command(OnOpenApplicationDataDirectoryExecute);
            BackupUserData = new Command(OnBackupUserDataExecute);

            Title = "Settings";
        }
        #endregion

        #region Properties
        public bool IsUpdateSystemAvailable { get; private set; }
        public bool CheckForUpdates { get; set; }
        public List<UpdateChannel> AvailableUpdateChannels { get; private set; }
        public UpdateChannel UpdateChannel { get; set; }
        #endregion

        #region Methods
        protected override async Task InitializeAsync()
        {
            await base.InitializeAsync();

            IsUpdateSystemAvailable = _updateService.IsUpdateSystemAvailable;
            CheckForUpdates = _updateService.CheckForUpdates;
            AvailableUpdateChannels = new List<UpdateChannel>(_updateService.AvailableChannels);
            UpdateChannel = _updateService.CurrentChannel;
        }

        protected override async Task<bool> SaveAsync()
        {
            _updateService.CheckForUpdates = CheckForUpdates;
            _updateService.CurrentChannel = UpdateChannel;

            return await base.SaveAsync();
        }
        #endregion

        #region Commands
        public Command OpenApplicationDataDirectory { get; private set; }

        private void OnOpenApplicationDataDirectoryExecute()
        {
            _manageUserDataService.OpenApplicationDataDirectory();
        }

        public Command BackupUserData { get; private set; }

        private void OnBackupUserDataExecute()
        {
            _manageUserDataService.BackupUserData();
        }
        #endregion
    }
}
