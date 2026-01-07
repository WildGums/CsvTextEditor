namespace CsvTextEditor
{
    using System;
    using System.Windows.Threading;
    using Catel.MVVM;
    using Catel.Services;
    using Catel.Windows.Threading;
    using Models;
    using Orc.CsvTextEditor;
    using Orc.ProjectManagement;

    public abstract class EditProjectCommandContainerBase : ProjectCommandContainerBase
    {
        private readonly ICsvTextEditorInstanceProvider _csvTextEditorInstanceProvider;
        private readonly DispatcherTimerEx _invalidateTimer;

        private ICsvTextEditorInstance _csvTextEditorInstance;

        protected EditProjectCommandContainerBase(string commandName, ICommandManager commandManager, 
            IProjectManager projectManager, ICsvTextEditorInstanceProvider csvTextEditorInstanceProvider, 
            IServiceProvider serviceProvider, IDispatcherService dispatcherService)
            : base(commandName, commandManager, projectManager, serviceProvider)
        {
            _csvTextEditorInstanceProvider = csvTextEditorInstanceProvider;

            _invalidateTimer = new DispatcherTimerEx(dispatcherService);
            _invalidateTimer.Interval = TimeSpan.FromMilliseconds(100);
            _invalidateTimer.Tick += OnInvalidateTimerTick;
        }        

        protected ICsvTextEditorInstance CsvTextEditorInstance
        {
            get
            {
                var activeProject = _projectManager.ActiveProject;
                _csvTextEditorInstance = _csvTextEditorInstanceProvider.GetInstance((Project)activeProject);

                return _csvTextEditorInstance;
            }
        }

        protected override void ProjectActivated(Project oldProject, Project newProject)
        {
            base.ProjectActivated(oldProject, newProject);

            if (_csvTextEditorInstance is not null && oldProject is not null)
            {
                _csvTextEditorInstance.TextChanged -= CsvTextEditorInstanceOnTextChanged;
            }

            if (newProject is null)
            {
                return;
            }

            _csvTextEditorInstance = _csvTextEditorInstanceProvider.GetInstance(newProject);

            if (_csvTextEditorInstance is not null)
            {
                _csvTextEditorInstance.TextChanged += CsvTextEditorInstanceOnTextChanged;
            }
        }

        private void CsvTextEditorInstanceOnTextChanged(object sender, EventArgs eventArgs)
        {
            _commandManager.InvalidateCommands();
        }

        public override bool CanExecute(object parameter)
        {
            if (!base.CanExecute(parameter))
            {
                return false;
            }

            var isEditorServiceNull = ReferenceEquals(CsvTextEditorInstance, null);
            if (isEditorServiceNull && !_invalidateTimer.IsEnabled)
            {
                _invalidateTimer.Start();
            }

            return !isEditorServiceNull;
        }

        private void OnInvalidateTimerTick(object sender, EventArgs e)
        {
            _invalidateTimer.Stop();
            
            _commandManager.InvalidateCommands();
        }
    }
}
