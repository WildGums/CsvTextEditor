namespace CsvTextEditor.ProjectManagement
{
    using System.Threading.Tasks;
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
