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
            Argument.IsNotNull(() => dispatcherService);

            _projectManager = projectManager;
            _dispatcherService = dispatcherService;
        }
        #endregion

        [Model]
        [Expose(nameof(Models.Project.Text))]
        public Project Project { get; set; }

        private void OnTextChanged()
        {
            var text = Project?.Text;
        }

        #region Methods
        protected override Task InitializeAsync()
        {
            _projectManager.ProjectActivationAsync += OnProjectActivationAsync;

            return base.InitializeAsync();
        }

        private async Task OnProjectActivationAsync(object sender, ProjectUpdatingCancelEventArgs e)
        {
            _dispatcherService.Invoke(() => Project = (Project)e.NewProject, true);
        }

        protected override Task CloseAsync()
        {
            _projectManager.ProjectActivationAsync -= OnProjectActivationAsync;

            return base.CloseAsync();
        }
        #endregion
    }
}