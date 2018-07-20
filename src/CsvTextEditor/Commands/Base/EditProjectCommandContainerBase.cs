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

        private ICsvTextEditorInstance _csvTextEditorInstance;
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

        protected ICsvTextEditorInstance CsvTextEditorInstance
        {
            get
            {
                var activeProject = _projectManager.ActiveProject;

                if (_csvTextEditorInstance == null && _serviceLocator.IsTypeRegistered<ICsvTextEditorInstance>(activeProject))
                {
                    _csvTextEditorInstance = _serviceLocator.ResolveType<ICsvTextEditorInstance>(activeProject);
                    
                }

                return _csvTextEditorInstance;
            }
        }

        protected override void ProjectActivated(Project oldProject, Project newProject)
        {
            base.ProjectActivated(oldProject, newProject);

            if (_csvTextEditorInstance != null)
            {
                _csvTextEditorInstance.TextChanged -= CsvTextEditorInstanceOnTextChanged;
            }

            if (newProject == null)
            {
                return;
            }

            if (_csvTextEditorInstance == null && _serviceLocator.IsTypeRegistered<ICsvTextEditorInstance>(newProject))
            {
                _csvTextEditorInstance = _serviceLocator.ResolveType<ICsvTextEditorInstance>(newProject);
            }

            if (_csvTextEditorInstance != null)
            {
                _csvTextEditorInstance.TextChanged += CsvTextEditorInstanceOnTextChanged;
            }
        }

        private void CsvTextEditorInstanceOnTextChanged(object sender, EventArgs eventArgs)
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
        #endregion
    }
}