namespace CsvTextEditor.ProjectManagement
{
    using System;
    using System.Threading.Tasks;
    using Catel;
    using Catel.IoC;
    using Catel.Services;
    using Catel.Threading;
    using CsvTextEditor.Models;
    using Orc.CsvTextEditor;
    using Orc.ProjectManagement;

    public class CsvTextEditorAutoCompleteProjectWatcher : ProjectWatcherBase
    {
        #region Fields
        private const int MaxLineCountWithAutoCompleteEnabled = 1000;

        private readonly IDispatcherService _dispatcherService;
        private readonly ICsvTextEditorInstanceProvider _csvTextEditorInstanceProvider;
        private ICsvTextEditorInstance _csvTextEditorInstance;
        #endregion

        #region Constructors
        public CsvTextEditorAutoCompleteProjectWatcher(IProjectManager projectManager,
            IDispatcherService dispatcherService, ICsvTextEditorInstanceProvider csvTextEditorInstanceProvider)
            : base(projectManager)
        {
            ArgumentNullException.ThrowIfNull(dispatcherService);
            ArgumentNullException.ThrowIfNull(csvTextEditorInstanceProvider);

            _dispatcherService = dispatcherService;
            _csvTextEditorInstanceProvider = csvTextEditorInstanceProvider;
        }
        #endregion

        protected override Task OnActivatedAsync(IProject oldProject, IProject newProject)
        {
            if (_csvTextEditorInstance is not null && oldProject is not null)
            {
                _csvTextEditorInstance.TextChanged -= CsvTextEditorInstanceOnTextChanged;
            }

            if (newProject is null)
            {
                return Task.CompletedTask;
            }

            _csvTextEditorInstance = _csvTextEditorInstanceProvider.GetInstance((Project)newProject);

            if (_csvTextEditorInstance is not null)
            {
                _csvTextEditorInstance.TextChanged += CsvTextEditorInstanceOnTextChanged;
            }

            _dispatcherService.Invoke(RefreshAutoComplete, true);

            return base.OnActivatedAsync(oldProject, newProject);
        }

        private void CsvTextEditorInstanceOnTextChanged(object sender, EventArgs e)
        {
            _dispatcherService.Invoke(RefreshAutoComplete, true);
        }

        private void RefreshAutoComplete()
        {
            _csvTextEditorInstance.IsAutocompleteEnabled = _csvTextEditorInstance.LinesCount <= MaxLineCountWithAutoCompleteEnabled;
        }
    }
}
