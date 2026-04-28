namespace CsvTextEditor.CommandLine
{
    using System.CommandLine;

    /// <summary>
    /// Start of an interface, but we don't want to create a full clone over System.CommandLine.RootCommand right now
    /// </summary>
    public interface IRootCommand
    {
        ParseResult Parse();
    }
}
