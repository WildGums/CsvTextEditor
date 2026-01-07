namespace CsvTextEditor.CommandLine
{
    using System.CommandLine;

    public class RootCommand : System.CommandLine.RootCommand, IRootCommand
    {
        private readonly ICommandLineProvider _commandLineProvider;

        public RootCommand(ICommandLineProvider commandLineProvider)
        {
            _commandLineProvider = commandLineProvider;

            var projectOption = new Option<string>(
                "--project",
                "-p")
            {
                Description = "The project to load"
            };

            Add(projectOption);
        }

        /// <summary>
        /// Parses the command line by using the injected command line provider.
        /// </summary>
        /// <returns></returns>
        public ParseResult Parse()
        {
            var commandLine = _commandLineProvider.GetCommandLine();

            return Parse(commandLine);
        }
    }
}
