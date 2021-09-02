// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MainViewModel.cs" company="WildGums">
//   Copyright (c) 2008 - 2017 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace CsvTextEditor.ViewModels
{
    using System.Threading.Tasks;
    using Catel;
    using Catel.Fody;
    using Catel.IoC;
    using Catel.MVVM;
    using Models;
    using Orc.CsvTextEditor;
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

        public ICsvTextEditorInstance CsvTextEditorInstance { get; set; }

        #region Methods
        protected override Task InitializeAsync()
        {
            _projectManager.ProjectActivationAsync += OnProjectActivationAsync;

            return base.InitializeAsync();
        }

        protected override Task OnClosedAsync(bool? result)
        {
            _projectManager.ProjectActivationAsync -= OnProjectActivationAsync;

            return base.OnClosedAsync(result);
        }

        private async Task OnProjectActivationAsync(object sender, ProjectUpdatingCancelEventArgs e)
        {
            var newProject = (Project)e.NewProject;
            Project = newProject;

            var serviceLocator = this.GetServiceLocator();
            var csvTextEditorInstanceProvider = serviceLocator.ResolveType<ICsvTextEditorInstanceProvider>();

            CsvTextEditorInstance = csvTextEditorInstanceProvider.GetInstance(Project);
            if (CsvTextEditorInstance.GetEditor() is not null)
            {
                CsvTextEditorInstance.SetInitialText(Project?.Text ?? string.Empty);
            }
        }

        protected override Task CloseAsync()
        {
            _projectManager.ProjectActivationAsync -= OnProjectActivationAsync;

            return base.CloseAsync();
        }
        #endregion
    }
}
