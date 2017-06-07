// --------------------------------------------------------------------------------------------------------------------
// <copyright file="EditProjectCommandContainerBase.cs" company="WildGums">
//   Copyright (c) 2008 - 2017 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace CsvTextEditor
{
    using System;
    using System.Windows.Threading;
    using Catel;
    using Catel.IoC;
    using Catel.MVVM;
    using Models;
    using Orc.CsvTextEditor;
    using Orc.ProjectManagement;

    public abstract class EditProjectCommandContainerBase : ProjectCommandContainerBase
    {
        #region Fields
        private readonly IServiceLocator _serviceLocator;
        private readonly DispatcherTimer _invalidateTimer;

        private ICsvTextEditorService _csvTextEditorService;
        #endregion

        #region Constructors
        protected EditProjectCommandContainerBase(string commandName, ICommandManager commandManager, IProjectManager projectManager, IServiceLocator serviceLocator)
            : base(commandName, commandManager, projectManager)
        {
            Argument.IsNotNull(() => serviceLocator);

            _serviceLocator = serviceLocator;

            _invalidateTimer = new DispatcherTimer();
            _invalidateTimer.Interval = TimeSpan.FromMilliseconds(100);
            _invalidateTimer.Tick += OnInvalidateTimerTick;
        }        
        #endregion

        protected ICsvTextEditorService CsvTextEditorService
        {
            get
            {
                var activeProject = _projectManager.ActiveProject;

                if (_csvTextEditorService == null && _serviceLocator.IsTypeRegistered<ICsvTextEditorService>(activeProject))
                {
                    _csvTextEditorService = _serviceLocator.ResolveType<ICsvTextEditorService>(activeProject);
                    
                }

                return _csvTextEditorService;
            }
        }

        protected override void ProjectActivated(Project oldProject, Project newProject)
        {
            base.ProjectActivated(oldProject, newProject);

            if (_csvTextEditorService != null)
            {
                _csvTextEditorService.TextChanged -= CsvTextEditorServiceOnTextChanged;
            }

            if (newProject == null)
            {
                return;
            }

            if (_csvTextEditorService == null && _serviceLocator.IsTypeRegistered<ICsvTextEditorService>(newProject))
            {
                _csvTextEditorService = _serviceLocator.ResolveType<ICsvTextEditorService>(newProject);
            }

            if (_csvTextEditorService != null)
            {
                _csvTextEditorService.TextChanged += CsvTextEditorServiceOnTextChanged;
            }
        }

        private void CsvTextEditorServiceOnTextChanged(object sender, EventArgs eventArgs)
        {
            _commandManager.InvalidateCommands();
        }

        #region Methods
        protected override bool CanExecute(object parameter)
        {
            if (!base.CanExecute(parameter))
            {
                return false;
            }

            var isEditorServiceNull = ReferenceEquals(CsvTextEditorService, null);
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
        #endregion
    }
}