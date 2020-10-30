// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FileOpenInExcelCommandContainer.cs" company="WildGums">
//   Copyright (c) 2008 - 2017 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace CsvTextEditor
{
    using Catel.MVVM;
    using Catel.Services;
    using Orc.FileSystem;
    using Orc.ProjectManagement;
    using Services;

    public class FileOpenInExcelCommandContainer : FileOpenInExternalToolCommandContainerBase
    {

        public FileOpenInExcelCommandContainer(ICommandManager commandManager, IProjectManager projectManager, IFileExtensionService fileExtensionService,
            IFileService fileService, IProcessService processService)
            : base(Commands.File.OpenInExcel, "xls", commandManager, projectManager, fileExtensionService, fileService, processService)
        {
        }
    }
}
