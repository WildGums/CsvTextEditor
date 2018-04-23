// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FileOpenInTextEditorCommandContainer.cs" company="WildGums">
//   Copyright (c) 2008 - 2017 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace CsvTextEditor
{
    using Catel.Configuration;
    using Catel.IoC;
    using Catel.MVVM;
    using Catel.Services;
    using Orc.FileSystem;
    using Orc.Notifications;
    using Orc.ProjectManagement;
    using Services;

    public class FileOpenInTextEditorCommandContainer : FileOpenInExternalToolCommandContainerBase
    {

        public FileOpenInTextEditorCommandContainer(ICommandManager commandManager, IProjectManager projectManager, IFileExtensionService fileExtensionService,
            IFileService fileService, IProcessService processService)
            : base(Commands.File.OpenInTextEditor , "txt", commandManager, projectManager, fileExtensionService, fileService, processService)
        {
        }


        protected override void Execute(object parameter)
        {

            var resolver = ServiceLocator.Default.GetDependencyResolver();
            var processService = resolver.Resolve<IProcessService>();
            var configurationService = resolver.Resolve<IConfigurationService>();
            var externalToolPath = configurationService.GetLocalValue<string>(Configuration.CustomEditor);

            if (externalToolPath != null)
            {
                processService.StartProcess(externalToolPath, _projectManager.ActiveProject.Location);
            } else
            {

                var fileExtensionService = resolver.Resolve<IFileExtensionService>();
                var defaultToolPath = fileExtensionService.GetRegisteredTool("txt");
                processService.StartProcess(defaultToolPath, _projectManager.ActiveProject.Location);

            }

            
        }
    }
}