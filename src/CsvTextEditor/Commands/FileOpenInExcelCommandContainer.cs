namespace CsvTextEditor
{
    using System;
    using Catel.MVVM;
    using Catel.Services;
    using Orc.FileSystem;
    using Orc.ProjectManagement;
    using Services;

    public class FileOpenInExcelCommandContainer : FileOpenInExternalToolCommandContainerBase
    {
        public FileOpenInExcelCommandContainer(ICommandManager commandManager, IProjectManager projectManager, IFileExtensionService fileExtensionService,
            IFileService fileService, IProcessService processService, IServiceProvider serviceProvider)
            : base(Commands.File.OpenInExcel, "xls", commandManager, projectManager, fileExtensionService, fileService, processService, serviceProvider)
        {
        }
    }
}
