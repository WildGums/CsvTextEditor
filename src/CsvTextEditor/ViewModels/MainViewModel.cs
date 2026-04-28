namespace CsvTextEditor.ViewModels
{
    using System;
    using System.Threading.Tasks;
    using Catel.Fody;
    using Catel.MVVM;
    using Models;
    using Orc.CsvTextEditor;
    using Orc.ProjectManagement;

    public class MainViewModel : FeaturedViewModelBase
    {
        private readonly IProjectManager _projectManager;
        private readonly ICsvTextEditorInstanceProvider _csvTextEditorInstanceProvider;
        private readonly ICsvTextEditorInstanceManager _csvTextEditorInstanceManager;

        public MainViewModel(IProjectManager projectManager, IServiceProvider serviceProvider,
            ICsvTextEditorInstanceProvider csvTextEditorInstanceProvider, 
            ICsvTextEditorInstanceManager csvTextEditorInstanceManager)
            : base(serviceProvider)
        {
            _projectManager = projectManager;
            _csvTextEditorInstanceProvider = csvTextEditorInstanceProvider;
            _csvTextEditorInstanceManager = csvTextEditorInstanceManager;
        }
       
        [Model]
        [Expose(nameof(Models.Project.Text))]
        public Project Project { get; set; }

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

#pragma warning disable IDISP001 //: Dispose created
            var instance = _csvTextEditorInstanceManager.GetInstance(Project.EditorId);
#pragma warning restore IDISP001 //: Dispose created
            if (instance?.GetEditor() is not null)
            {
                instance.SetInitialText(Project?.Text ?? string.Empty);
            }
        }

        protected override Task CloseAsync()
        {
            _projectManager.ProjectActivationAsync -= OnProjectActivationAsync;

            return base.CloseAsync();
        }
    }
}
