namespace CsvTextEditor.ProjectManagement
{
    using System;
    using System.Threading.Tasks;
    using Models;
    using Orc.FileSystem;
    using Orc.ProjectManagement;

    public class ProjectWriter : ProjectWriterBase<Project>
    {
        private readonly IFileService _fileService;
        private readonly ICsvTextEditorInstanceProvider _csvTextEditorInstanceProvider;

        public ProjectWriter(IFileService fileService, ICsvTextEditorInstanceProvider csvTextEditorInstanceProvider)
        {
            ArgumentNullException.ThrowIfNull(fileService);
            ArgumentNullException.ThrowIfNull(csvTextEditorInstanceProvider);

            _fileService = fileService;
            _csvTextEditorInstanceProvider = csvTextEditorInstanceProvider;
        }

        protected override Task<bool> WriteToLocationAsync(Project project, string location)
        {
            _fileService.WriteAllText(location, project.Text);

            var csvTextEditorInstance = _csvTextEditorInstanceProvider.GetInstance(project);
            csvTextEditorInstance.ResetIsDirty();

            return Task.FromResult<bool>(true);
        }
    }
}
