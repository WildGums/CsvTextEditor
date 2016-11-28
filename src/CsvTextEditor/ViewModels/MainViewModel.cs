// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MainViewModel.cs" company="WildGums">
//   Copyright (c) 2008 - 2016 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace CsvTextEditor.ViewModels
{
    using System;
    using System.Threading.Tasks;
    using Catel;
    using Catel.Fody;
    using Catel.MVVM;
    using Catel.Services;
    using Catel.Threading;
    using Models;
    using Orc.ProjectManagement;

    public class MainViewModel : ViewModelBase
    {
        #region Fields
        private readonly IProjectManager _projectManager;
        private readonly IDispatcherService _dispatcherService;
        #endregion

        #region Constructors
        public MainViewModel(IProjectManager projectManager, IDispatcherService dispatcherService)
        {
            Argument.IsNotNull(() => projectManager);

            _projectManager = projectManager;
            _dispatcherService = dispatcherService;
        }
        #endregion

        [Model]
        [Expose(nameof(Models.Project.Text))]
        public Project Project { get; set; }

        #region Methods
        protected override async Task InitializeAsync()
        {
            await base.InitializeAsync();

            _projectManager.ProjectActivationAsync += OnProjectActivationAsync;
        }

        private async Task OnProjectActivationAsync(object sender, ProjectUpdatingCancelEventArgs e)
        {
            _dispatcherService.Invoke(() => Project = e.NewProject as Project, true);
        }

        protected override async Task CloseAsync()
        {
            await base.CloseAsync();

            _projectManager.ProjectActivationAsync -= OnProjectActivationAsync;
        }
        #endregion
    }
}