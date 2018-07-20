// --------------------------------------------------------------------------------------------------------------------
// <copyright file="InitialProjectLocationService.cs" company="WildGums">
//   Copyright (c) 2008 - 2018 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace CsvTextEditor.Services
{
    using System.IO;
    using System.Threading.Tasks;
    using Catel;
    using Catel.Configuration;
    using Catel.Logging;
    using Catel.Services;
    using Orc.CommandLine;
    using Orc.FileSystem;

    public class InitialProjectLocationService : Orc.ProjectManagement.IInitialProjectLocationService
    {
        #region Fields
        private static readonly ILog Log = LogManager.GetCurrentClassLogger();

        private readonly IConfigurationService _configurationService;
        private readonly IDirectoryService _directoryService;
        private readonly ICommandLineParser _commandLineParser;
        private readonly IFileService _fileService;
        private readonly IStartUpInfoProvider _startUpInfoProvider;
        #endregion

        #region Constructors
        public InitialProjectLocationService(IConfigurationService configurationService, IStartUpInfoProvider startUpInfoProvider,
            IFileService fileService, IDirectoryService directoryService, ICommandLineParser commandLineParser)
        {
            Argument.IsNotNull(() => configurationService);
            Argument.IsNotNull(() => startUpInfoProvider);
            Argument.IsNotNull(() => fileService);
            Argument.IsNotNull(() => directoryService);
            Argument.IsNotNull(() => commandLineParser);

            _configurationService = configurationService;
            _startUpInfoProvider = startUpInfoProvider;
            _fileService = fileService;
            _directoryService = directoryService;
            _commandLineParser = commandLineParser;
        }
        #endregion

        #region Methods
        public async Task<string> GetInitialProjectLocationAsync()
        {
            var commandLineContext = new CommandLineContext();
            _commandLineParser.Parse(_startUpInfoProvider.Arguments, commandLineContext);

            return commandLineContext.InitialFile;
        }
        #endregion
    }
}