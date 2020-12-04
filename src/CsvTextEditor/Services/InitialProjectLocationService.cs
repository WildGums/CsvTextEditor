// --------------------------------------------------------------------------------------------------------------------
// <copyright file="InitialProjectLocationService.cs" company="WildGums">
//   Copyright (c) 2008 - 2018 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace CsvTextEditor.Services
{
    using System.Threading.Tasks;
    using Catel;
    using Catel.Logging;
    using Orc.CommandLine;

    public class InitialProjectLocationService : Orc.ProjectManagement.IInitialProjectLocationService
    {
        #region Fields
        private static readonly ILog Log = LogManager.GetCurrentClassLogger();

        private readonly ICommandLineParser _commandLineParser;
        private readonly ICommandLineService _commandLineService;
        #endregion

        #region Constructors
        public InitialProjectLocationService(ICommandLineService commandLineService, ICommandLineParser commandLineParser)
        {
            Argument.IsNotNull(() => commandLineService);
            Argument.IsNotNull(() => commandLineParser);

            _commandLineService = commandLineService;
            _commandLineParser = commandLineParser;
        }
        #endregion

        #region Methods
        public async Task<string> GetInitialProjectLocationAsync()
        {
            var commandLineContext = new CommandLineContext();
            _commandLineParser.Parse(_commandLineService.GetCommandLine(), commandLineContext);

            return commandLineContext.InitialFile;
        }
        #endregion
    }
}
