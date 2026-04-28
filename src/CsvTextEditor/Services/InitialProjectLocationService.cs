namespace CsvTextEditor.Services
{
    using System.Threading.Tasks;
    using Catel.Logging;
    using CsvTextEditor.CommandLine;
    using Microsoft.Extensions.Logging;

    public class InitialProjectLocationService : Orc.ProjectManagement.IInitialProjectLocationService
    {
        private readonly IRootCommand _rootCommand;

        public InitialProjectLocationService(IRootCommand rootCommand)
        {
            _rootCommand = rootCommand;
        }

        public async Task<string?> GetInitialProjectLocationAsync()
        {
            var context = _rootCommand.GetProjectCommandContext();

            return context.Project;
        }
    }
}
