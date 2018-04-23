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
        #endregion

        #region Constructors
        public ProjectReader(IFileService fileService)
        {
            Argument.IsNotNull(() => fileService);

            _fileService = fileService;
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

            } catch (System.IO.IOException e)
            {

                

                var resolver = ServiceLocator.Default.GetDependencyResolver();
                var notificationService = resolver.Resolve<INotificationService>();
                notificationService.ShowNotification("Could not open file", e.Message);
            }

            return null;

        }
        #endregion
    }
}