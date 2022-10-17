namespace CsvTextEditor.ProjectManagement
{
    using System;
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
        private readonly IFileService _fileService;
        private readonly INotificationService _notificationService;

        public ProjectReader(IFileService fileService, INotificationService notificationService)
        {
            ArgumentNullException.ThrowIfNull(fileService);
            ArgumentNullException.ThrowIfNull(notificationService);

            _fileService = fileService;
            _notificationService = notificationService;
        }

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
    }
}
