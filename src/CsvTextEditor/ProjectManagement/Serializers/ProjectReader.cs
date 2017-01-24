// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ProjectReader.cs" company="WildGums">
//   Copyright (c) 2008 - 2017 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace CsvTextEditor.ProjectManagement
{
    using System.Threading.Tasks;
    using Catel;
    using Models;
    using Orc.FileSystem;
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
            var text = await _fileService.ReadAllTextAsync(location);

            var project = new Project(location)
            {
                Text = text
            };

            return project;
        }
        #endregion
    }
}