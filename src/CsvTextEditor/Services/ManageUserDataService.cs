// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ManageUserDataService.cs" company="WildGums">
//   Copyright (c) 2008 - 2017 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace CsvTextEditor.Services
{
    using System.Linq;
    using System.Threading.Tasks;
    using Catel;
    using Catel.Logging;
    using Catel.Services;
    using Orc.FileSystem;
    using Orchestra.Services;

    public class ManageUserDataService : AppDataService, IManageUserDataService
    {
        private static readonly ILog Log = LogManager.GetCurrentClassLogger();

        public ManageUserDataService(IMessageService messageService, ISaveFileService saveFileService, IProcessService processService, 
            IDirectoryService directoryService, IFileService fileService)
            : base(messageService, saveFileService, processService, directoryService, fileService)
        {
            
        }
    }
}
