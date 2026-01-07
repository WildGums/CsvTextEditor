namespace CsvTextEditor
{
    using System;
    using Catel.Configuration;
    using Catel.MVVM;
    using Catel.Services;
    using Orc.FileSystem;
    using Orc.ProjectManagement;
    using Services;

    public class FileOpenInTextEditorCommandContainer : FileOpenInExternalToolCommandContainerBase
    {
        private readonly IFileExtensionService _fileExtensionService;
        private readonly IProcessService _processService;
        private readonly IConfigurationService _configurationService;

        public FileOpenInTextEditorCommandContainer(ICommandManager commandManager, IProjectManager projectManager, IFileExtensionService fileExtensionService,
            IFileService fileService, IProcessService processService, IConfigurationService configurationService, IServiceProvider serviceProvider)
            : base(Commands.File.OpenInTextEditor, "txt", commandManager, projectManager, fileExtensionService, fileService, processService, serviceProvider)
        {
            _fileExtensionService = fileExtensionService;
            _processService = processService;
            _configurationService = configurationService;
        }

        public override void Execute(object parameter)
        {
            var externalToolPath = _configurationService.GetRoamingValue<string>(Configuration.CustomEditor, null);

            if (string.Equals(externalToolPath, "null"))
            {
                externalToolPath = null;
            }

            var toolPath = !string.IsNullOrEmpty(externalToolPath) ? externalToolPath : _fileExtensionService.GetRegisteredTool("txt");

            if (!string.IsNullOrEmpty(toolPath))
            {
                _processService.StartProcess(toolPath, _projectManager.ActiveProject.Location);
            }
            else
            {
                _processService.StartProcess(new ProcessContext
                {
                    FileName = "notepad.exe",
                    Arguments = _projectManager.ActiveProject.Location,
                    UseShellExecute = true
                });
            }
        }
    }
}
