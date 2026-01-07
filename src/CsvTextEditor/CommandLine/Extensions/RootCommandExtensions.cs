namespace CsvTextEditor.CommandLine
{
    public static class RootCommandExtensions
    {
        public static ProjectCommandContext GetProjectCommandContext(this IRootCommand rootCommand)
        {
            var parseResult = rootCommand.Parse();

            var context = new ProjectCommandContext
            {
                Project = parseResult.GetValue<string?>("--project")
            };

            return context;
        }
    }
}
