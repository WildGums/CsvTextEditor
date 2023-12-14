namespace CsvTextEditor.Services
{
    using System;
    using System.Threading.Tasks;
    using Catel;
    using Catel.Logging;
    using Orc.CommandLine;

    public class InitialProjectLocationService : Orc.ProjectManagement.IInitialProjectLocationService
    {
        private static readonly ILog Log = LogManager.GetCurrentClassLogger();

        private readonly ICommandLineParser _commandLineParser;
        private readonly ICommandLineService _commandLineService;

        public InitialProjectLocationService(ICommandLineService commandLineService, ICommandLineParser commandLineParser)
        {
            ArgumentNullException.ThrowIfNull(commandLineService);
            ArgumentNullException.ThrowIfNull(commandLineParser);

            _commandLineService = commandLineService;
            _commandLineParser = commandLineParser;
        }

        public async Task<string> GetInitialProjectLocationAsync()
        {
            var commandLineContext = new CommandLineContext();
            _commandLineParser.Parse(_commandLineService.GetCommandLine(), commandLineContext);

            return commandLineContext.InitialFile;
        }
    }
}
