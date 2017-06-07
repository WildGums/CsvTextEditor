// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ProjectWriter.cs" company="WildGums">
//   Copyright (c) 2008 - 2017 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace CsvTextEditor.ProjectManagement
{
    using System.Threading.Tasks;
    using Catel;
    using Catel.Threading;
    using Models;
    using Orc.FileSystem;
    using Orc.ProjectManagement;

    public class ProjectWriter : ProjectWriterBase<Project>
    {
        #region Fields
        private readonly IFileService _fileService;
        #endregion

        #region Constructors
        public ProjectWriter(IFileService fileService)
        {
            Argument.IsNotNull(() => fileService);

            _fileService = fileService;
        }
        #endregion

        #region Methods
        protected override Task<bool> WriteToLocationAsync(Project project, string location)
        {
            _fileService.WriteAllText(location, project.Text);

            return TaskHelper<bool>.FromResult(true);
        }
        #endregion
    }
}