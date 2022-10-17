namespace CsvTextEditor
{
    using System;
    using System.IO;
    using System.Threading.Tasks;
    using Catel;
    using Catel.MVVM;
    using Catel.Services;
    using Models;
    using Orc.ProjectManagement;

    public class FileSaveAsCommandContainer : ProjectCommandContainerBase
    {
        private readonly ISaveFileService _saveFileService;

        public FileSaveAsCommandContainer(ICommandManager commandManager, IProjectManager projectManager, ISaveFileService saveFileService)
            : base(Commands.File.SaveAs, commandManager, projectManager)
        {
            ArgumentNullException.ThrowIfNull(saveFileService);

            _saveFileService = saveFileService;
        }

        protected override async Task ExecuteAsync(object parameter)
        {
            if (!(_projectManager.ActiveProject is Project project))
            {
                return;
            }

            var result = await _saveFileService.DetermineFileAsync(new DetermineSaveFileContext
            {
                Filter = "Text Files (*.csv)|*csv",
                AddExtension = true
            });

            if (result.Result)
            {
                var fileName = result.FileName;

                // Note: manually ensure we are using correct extension
                fileName = Path.ChangeExtension(fileName, "csv");

                await _projectManager.SaveAsync(project, fileName);
            }

            await base.ExecuteAsync(parameter);
        }
    }
}
