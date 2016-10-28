// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MainViewModel.cs" company="WildGums">
//   Copyright (c) 2008 - 2016 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace CsvTextEditor.ViewModels
{
    using System.Threading.Tasks;
    using Catel;
    using Catel.Fody;
    using Catel.MVVM;
    using Catel.Threading;
    using Models;
    using Orc.ProjectManagement;

    public class MainViewModel : ViewModelBase
    {
        #region Fields
        private readonly IProjectManager _projectManager;
        #endregion

        #region Constructors
        public MainViewModel(IProjectManager projectManager)
        {
            Argument.IsNotNull(() => projectManager);

            _projectManager = projectManager;
        }
        #endregion

        [Model]
        [Expose(nameof(Models.Project.Text))]
        public Project Project { get; set; }

        #region Methods
        protected override Task InitializeAsync()
        {
            _projectManager.ProjectActivatedAsync += OnProjectActivated;

            return base.InitializeAsync();
        }

        protected override Task CloseAsync()
        {
            _projectManager.ProjectActivatedAsync -= OnProjectActivated;

            return base.CloseAsync();
        }

        private Task OnProjectActivated(object sender, ProjectUpdatedEventArgs e)
        {
            var project = e.NewProject as Project;

            if (ReferenceEquals(project, null))
            {
                Project = null;

                return TaskHelper.Completed;
            }

            Project = project;

            return TaskHelper.Completed;
        }
        #endregion
    }
}