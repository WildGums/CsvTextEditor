namespace CsvTextEditor.ViewModels
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using Catel.MVVM;
    using Catel.Reflection;
    using Catel.Services;
    using Models;
    using Orc.ProjectManagement;
    using Orchestra;
    using Orchestra.ViewModels;

    public class RibbonViewModel : ViewModelBase
    {
        private readonly IProjectManager _projectManager;
        private readonly ICommandManager _commandManager;
        private readonly ICommandInfoService _commandInfoService;
        private readonly IUIVisualizerService _uiVisualizerService;

        public RibbonViewModel(IUIVisualizerService uiVisualizerService, IProjectManager projectManager, 
            ICommandManager commandManager, ICommandInfoService commandInfoService, IServiceProvider serviceProvider)
            : base(serviceProvider)
        {
            _uiVisualizerService = uiVisualizerService;
            _projectManager = projectManager;
            _commandManager = commandManager;
            _commandInfoService = commandInfoService;

            ShowKeyboardMappings = new TaskCommand(serviceProvider, OnShowKeyboardMappingsExecuteAsync);

            Title = AssemblyHelper.GetEntryAssembly().Title();

            _projectManager.ProjectActivatedAsync += OnProjectActivatedAsync;
        }

        public Project Project { get; private set; }       

        private Task OnProjectActivatedAsync(object sender, ProjectUpdatedEventArgs e)
        {
            Project = (Project)e.NewProject;

            return Task.CompletedTask;
        }

        public TaskCommand ShowKeyboardMappings { get; }

        private async Task OnShowKeyboardMappingsExecuteAsync()
        {
            var allCommands = _commandManager.GetCommands().OrderBy(x => x).ToList();

            foreach (var command in allCommands)
            {
                var commandInfo = _commandInfoService.GetCommandInfo(command);
            }

            await _uiVisualizerService.ShowDialogAsync<KeyboardMappingsCustomizationViewModel>();
        }
    }
}
