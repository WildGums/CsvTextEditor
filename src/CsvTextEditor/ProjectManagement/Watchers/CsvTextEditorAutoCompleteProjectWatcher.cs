// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CsvTextEditorAutoCompleteProjectWatcher.cs" company="WildGums">
//   Copyright (c) 2008 - 2017 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace CsvTextEditor.ProjectManagement.Watchers
{
    using System;
    using System.Threading.Tasks;
    using Catel;
    using Catel.IoC;
    using Catel.Services;
    using Catel.Threading;
    using Orc.CsvTextEditor.Services;
    using Orc.ProjectManagement;

    public class CsvTextEditorAutoCompleteProjectWatcher : ProjectWatcherBase
    {
        #region Fields
        private const int MaxLineCountWithAutoCompleteEnabled = 1000;

        private readonly IDispatcherService _dispatcherService;
        private readonly IServiceLocator _serviceLocator;
        private ICsvTextEditorService _csvTextEditorService;
        #endregion

        #region Constructors
        public CsvTextEditorAutoCompleteProjectWatcher(IProjectManager projectManager, IServiceLocator serviceLocator,
            IDispatcherService dispatcherService)
            : base(projectManager)
        {
            Argument.IsNotNull(() => serviceLocator);
            Argument.IsNotNull(() => dispatcherService);

            _serviceLocator = serviceLocator;
            _dispatcherService = dispatcherService;
        }
        #endregion

        protected override Task OnActivatedAsync(IProject oldProject, IProject newProject)
        {
            if (_csvTextEditorService != null)
            {
                _csvTextEditorService.TextChanged -= CsvTextEditorServiceOnTextChanged;
            }

            if (newProject == null)
            {
                return TaskHelper.Completed;
            }

            if (_csvTextEditorService == null && _serviceLocator.IsTypeRegistered<ICsvTextEditorService>(newProject))
            {
                _csvTextEditorService = _serviceLocator.ResolveType<ICsvTextEditorService>(newProject);
            }

            if (_csvTextEditorService != null)
            {
                _csvTextEditorService.TextChanged += CsvTextEditorServiceOnTextChanged;
            }

            _dispatcherService.Invoke(RefreshAutoComplete, true);

            return base.OnActivatedAsync(oldProject, newProject);
        }

        private void CsvTextEditorServiceOnTextChanged(object sender, EventArgs e)
        {
            _dispatcherService.Invoke(RefreshAutoComplete, true);
        }

        private void RefreshAutoComplete()
        {
            _csvTextEditorService.IsAutocompleteEnabled = _csvTextEditorService.LineCount <= MaxLineCountWithAutoCompleteEnabled;
        }
    }
}