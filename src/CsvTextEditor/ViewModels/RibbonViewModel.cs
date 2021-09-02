// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RibbonViewModel.cs" company="WildGums">
//   Copyright (c) 2008 - 2017 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace CsvTextEditor.ViewModels
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using Catel;
    using Catel.MVVM;
    using Catel.Reflection;
    using Catel.Services;
    using Catel.Threading;
    using Models;
    using Orc.ProjectManagement;
    using Orchestra.Services;
    using Orchestra.ViewModels;

    public class RibbonViewModel : ViewModelBase
    {
        #region Fields
        private readonly IProjectManager _projectManager;
        private readonly ICommandManager _commandManager;
        private readonly ICommandInfoService _commandInfoService;
        private readonly IUIVisualizerService _uiVisualizerService;
        #endregion

        #region Constructors
        public RibbonViewModel(IUIVisualizerService uiVisualizerService, IProjectManager projectManager, ICommandManager commandManager, ICommandInfoService commandInfoService)
        {
            Argument.IsNotNull(() => uiVisualizerService);
            Argument.IsNotNull(() => projectManager);

            _uiVisualizerService = uiVisualizerService;
            _projectManager = projectManager;
            _commandManager = commandManager;
            _commandInfoService = commandInfoService;

            ShowKeyboardMappings = new TaskCommand(OnShowKeyboardMappingsExecuteAsync);

            Title = AssemblyHelper.GetEntryAssembly().Title();

            _projectManager.ProjectActivatedAsync += OnProjectActivatedAsync;
        }
        #endregion

        public Project Project { get; private set; }       

        #region Methods
        private Task OnProjectActivatedAsync(object sender, ProjectUpdatedEventArgs e)
        {
            Project = (Project)e.NewProject;

            return TaskHelper.Completed;
        }
        #endregion

        #region Commands
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
        #endregion
    }
}
