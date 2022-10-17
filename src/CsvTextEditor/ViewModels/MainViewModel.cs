namespace CsvTextEditor.ViewModels
{
    using System;
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
        private readonly IProjectManager _projectManager;

        public MainViewModel(IProjectManager projectManager)
        {
            ArgumentNullException.ThrowIfNull(projectManager);

            _projectManager = projectManager;
        }
       
        [Model]
        [Expose(nameof(Models.Project.Text))]
        public Project Project { get; set; }

        public ICsvTextEditorInstance CsvTextEditorInstance { get; set; }

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

#pragma warning disable IDISP001 // Dispose created
            var serviceLocator = this.GetServiceLocator();
#pragma warning restore IDISP001 // Dispose created
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
    }
}
