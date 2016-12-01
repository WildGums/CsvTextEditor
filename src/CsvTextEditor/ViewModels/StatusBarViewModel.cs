// --------------------------------------------------------------------------------------------------------------------
// <copyright file="StatusBarViewModel.cs" company="WildGums">
//   Copyright (c) 2008 - 2016 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace CsvTextEditor.ViewModels
{
    using System.Threading.Tasks;
    using Catel;
    using Catel.IoC;
    using Catel.MVVM;
    using Catel.Threading;
    using Orc.CsvTextEditor;
    using Orc.CsvTextEditor.Services;
    using Orc.ProjectManagement;
    using Orchestra;

    public class StatusBarViewModel : ViewModelBase
    {
        #region Fields
        private readonly IProjectManager _projectManager;
        private readonly IServiceLocator _serviceLocator;
        private ICsvTextEditorService _csvTextEditorService;
        #endregion

        #region Constructors
        public StatusBarViewModel(IProjectManager projectManager, IServiceLocator serviceLocator)
        {
            Argument.IsNotNull(() => projectManager);
            Argument.IsNotNull(() => serviceLocator);

            _projectManager = projectManager;
            _serviceLocator = serviceLocator;
        }
        #endregion

        #region Properties
        public string Version { get; private set; }
        public int Column { get; private set; }
        public int Line { get; private set; }
        #endregion

        protected override Task InitializeAsync()
        {
            Version = VersionHelper.GetCurrentVersion();

            _projectManager.ProjectActivatedAsync += OnProjectActivatedAsync;

            return base.InitializeAsync();
        }

        protected override Task CloseAsync()
        {
            _projectManager.ProjectActivatedAsync -= OnProjectActivatedAsync;

            return base.CloseAsync();
        }

        private Task OnProjectActivatedAsync(object sender, ProjectUpdatedEventArgs args)
        {
            if (_csvTextEditorService != null)
            {
                _csvTextEditorService.CaretTextLocationChanged -= OnCaretTextLocationChanged;
            }

            var activeProject = args.NewProject;
            if (activeProject == null)
            {
                return TaskHelper.Completed;
            }

            _csvTextEditorService = _serviceLocator.ResolveType<ICsvTextEditorService>(args.NewProject);

            _csvTextEditorService.CaretTextLocationChanged += OnCaretTextLocationChanged;

            return TaskHelper.Completed;
        }

        private void OnCaretTextLocationChanged(object sender, CaretTextLocationChangedEventArgs args)
        {
            Column = args.Column;
            Line = args.Line;
        }
    }
}