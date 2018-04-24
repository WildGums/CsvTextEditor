// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FileOpenInTextEditorCommandContainer.cs" company="WildGums">
//   Copyright (c) 2008 - 2017 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace CsvTextEditor
{
    using Catel;
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
            IFileService fileService, IProcessService processService, IConfigurationService configurationService)
            : base(Commands.File.OpenInTextEditor, "txt", commandManager, projectManager, fileExtensionService, fileService, processService)
        {
            Argument.IsNotNull(() => fileExtensionService);
            Argument.IsNotNull(() => configurationService);
            Argument.IsNotNull(() => processService);

            _fileExtensionService = fileExtensionService;
            _processService = processService;
            _configurationService = configurationService;
        }

        protected override void Execute(object parameter)
        {
            var externalToolPath = _configurationService.GetLocalValue<string>(Configuration.CustomEditor);

            if (externalToolPath != null)
            {
                _processService.StartProcess(externalToolPath, _projectManager.ActiveProject.Location);
            }
            else
            {
                var defaultToolPath = _fileExtensionService.GetRegisteredTool("txt");
                _processService.StartProcess(defaultToolPath, _projectManager.ActiveProject.Location);
            }
        }
    }
}