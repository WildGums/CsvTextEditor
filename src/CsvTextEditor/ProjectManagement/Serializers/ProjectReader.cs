// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ProjectReader.cs" company="WildGums">
//   Copyright (c) 2008 - 2017 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace CsvTextEditor.ProjectManagement
{
    using System.Diagnostics;
    using System.Threading.Tasks;
    using Catel;
    using Catel.IoC;
    using Catel.Services;
    using Models;
    using Orc.FileSystem;
    using Orc.Notifications;
    using Orc.ProjectManagement;

    public class ProjectReader : ProjectReaderBase
    {
        #region Fields
        private readonly IFileService _fileService;
        private readonly INotificationService _notificationService;
        #endregion

        #region Constructors
        public ProjectReader(IFileService fileService, INotificationService notificationService)
        {
            Argument.IsNotNull(() => fileService);
            Argument.IsNotNull(() => notificationService);

            _fileService = fileService;
            _notificationService = notificationService;
        }
        #endregion

        #region Methods
        protected override async Task<IProject> ReadFromLocationAsync(string location)
        {

            try
            {
                var text = await _fileService.ReadAllTextAsync(location);

                var project = new Project(location)
                {
                    Text = text
                };

                return project;

            } catch (System.IO.IOException ex)
            {
                _notificationService.ShowNotification("Could not open file", ex.Message);
            }

            return null;

        }
        #endregion
    }
}