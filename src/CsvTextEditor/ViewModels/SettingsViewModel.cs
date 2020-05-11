// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SettingsViewModel.cs" company="WildGums">
//   Copyright (c) 2008 - 2017 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace CsvTextEditor.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Threading.Tasks;
    using Catel;
    using Catel.Configuration;
    using Catel.IoC;
    using Catel.MVVM;
    using Catel.Services;
    using Orc.Squirrel;
    using Orchestra.Services;
    using Services;

    public class SettingsViewModel : ViewModelBase
    {
        #region Fields
        private readonly IConfigurationService _configurationService;
        private readonly IManageAppDataService _manageAppDataService;
        private readonly IUpdateService _updateService;
        private readonly IOpenFileService _openFileService;
        #endregion

        #region Constructors
        public SettingsViewModel(IConfigurationService configurationService, IManageAppDataService manageAppDataService, IUpdateService updateService, IOpenFileService openFileService)
        {
            Argument.IsNotNull(() => configurationService);
            Argument.IsNotNull(() => manageAppDataService);
            Argument.IsNotNull(() => updateService);
            Argument.IsNotNull(() => openFileService);

            _configurationService = configurationService;
            _manageAppDataService = manageAppDataService;
            _updateService = updateService;
            _openFileService = openFileService;

            PickEditor = new TaskCommand(PickEditorExecuteAsync);
            OpenApplicationDataDirectory = new Command(OnOpenApplicationDataDirectoryExecute);
            BackupUserData = new TaskCommand(OnBackupUserDataExecuteAsync);

            Title = "Settings";
        }
        #endregion

        #region Properties
        public bool IsUpdateSystemAvailable { get; private set; }
        public bool CheckForUpdates { get; set; }
        public bool AutoSaveEditor { get; set; }
        public string CustomEditor { get; private set; }
        public List<UpdateChannel> AvailableUpdateChannels { get; private set; }
        public UpdateChannel UpdateChannel { get; set; }
        #endregion

        #region Commands
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
        #endregion

        #region Methods
        protected override async Task InitializeAsync()
        {
            await base.InitializeAsync();

            IsUpdateSystemAvailable = _updateService.IsUpdateSystemAvailable;
            CheckForUpdates = _updateService.CheckForUpdates;
            AvailableUpdateChannels = new List<UpdateChannel>(_updateService.AvailableChannels);
            UpdateChannel = _updateService.CurrentChannel;

            CustomEditor = _configurationService.GetRoamingValue<string>(Configuration.CustomEditor);
            AutoSaveEditor = _configurationService.GetRoamingValue(Configuration.AutoSaveEditor, Configuration.AutoSaveEditorDefaultValue);
        }

        protected override async Task<bool> SaveAsync()
        {
            _updateService.CheckForUpdates = CheckForUpdates;
            _updateService.CurrentChannel = UpdateChannel;

            _configurationService.SetRoamingValue(Configuration.CustomEditor, CustomEditor);
            _configurationService.SetRoamingValue(Configuration.AutoSaveEditor, AutoSaveEditor);

            return await base.SaveAsync();
        }
        #endregion
    }
}
