// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ProjectWriter.cs" company="WildGums">
//   Copyright (c) 2008 - 2017 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace CsvTextEditor.ProjectManagement
{
    using System.Threading.Tasks;
    using Catel;
    using Catel.IoC;
    using Catel.Threading;
    using Models;
    using Orc.CsvTextEditor;
    using Orc.FileSystem;
    using Orc.ProjectManagement;

    public class ProjectWriter : ProjectWriterBase<Project>
    {
        #region Fields
        private readonly IFileService _fileService;
        private readonly ICsvTextEditorInstanceProvider _csvTextEditorInstanceProvider;
        #endregion

        #region Constructors
        public ProjectWriter(IFileService fileService, ICsvTextEditorInstanceProvider csvTextEditorInstanceProvider)
        {
            Argument.IsNotNull(() => fileService);
            Argument.IsNotNull(() => csvTextEditorInstanceProvider);

            _fileService = fileService;
            _csvTextEditorInstanceProvider = csvTextEditorInstanceProvider;
        }
        #endregion

        #region Methods
        protected override Task<bool> WriteToLocationAsync(Project project, string location)
        {
            _fileService.WriteAllText(location, project.Text);

            var csvTextEditorInstance = _csvTextEditorInstanceProvider.GetInstance(project);
            csvTextEditorInstance.ResetIsDirty();

            return TaskHelper<bool>.FromResult(true);
        }
        #endregion
    }
}
