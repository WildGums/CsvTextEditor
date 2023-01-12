namespace CsvTextEditor
{
    using Orc.CommandLine;

    public class CommandLineContext : ContextBase
    {
        [Option("", "", DisplayName = "initialFile", HelpText = "The initial file to open in CsvTextEditor")]
        public string InitialFile { get; set; }
    }
}
