// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RibbonViewModel.cs" company="WildGums">
//   Copyright (c) 2008 - 2017 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace CsvTextEditor.ViewModels
{
    using System.Threading.Tasks;
    using Catel;
    using Catel.MVVM;
    using Catel.Reflection;
    using Catel.Services;
    using Catel.Threading;
    using Models;
    using Orc.ProjectManagement;
    using Orchestra.ViewModels;

    public class RibbonViewModel : ViewModelBase
    {
        #region Fields
        private readonly IProjectManager _projectManager;
        private readonly IUIVisualizerService _uiVisualizerService;
        #endregion

        #region Constructors
        public RibbonViewModel(IUIVisualizerService uiVisualizerService, IProjectManager projectManager)
        {
            Argument.IsNotNull(() => uiVisualizerService);
            Argument.IsNotNull(() => projectManager);

            _uiVisualizerService = uiVisualizerService;
            _projectManager = projectManager;

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
        public TaskCommand ShowKeyboardMappings { get; private set; }

        private async Task OnShowKeyboardMappingsExecuteAsync()
        {
            _uiVisualizerService.ShowDialog<KeyboardMappingsCustomizationViewModel>();
        }
        #endregion
    }
}