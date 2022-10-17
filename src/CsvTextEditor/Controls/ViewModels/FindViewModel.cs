namespace CsvTextEditor.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Linq;
    using System.Threading.Tasks;
    using Catel;
    using Catel.Data;
    using Catel.MVVM;
    using Models;
    using Orc.CsvTextEditor;
    using Orc.ProjectManagement;

    public class FindViewModel : ViewModelBase
    {
        #region Fields
        private readonly ICsvTextEditorInstanceProvider _csvTextEditorInstanceProvider;
        private readonly IProjectManager _projectManager;

        private ICsvTextEditorInstance _csvTextEditorInstance;
        #endregion

        #region Constructors
        public FindViewModel(ICsvTextEditorInstanceProvider csvTextEditorInstanceProvider, IProjectManager projectManager)
        {
            ArgumentNullException.ThrowIfNull(csvTextEditorInstanceProvider);
            ArgumentNullException.ThrowIfNull(projectManager);

            _csvTextEditorInstanceProvider = csvTextEditorInstanceProvider;
            _projectManager = projectManager;
        }
        #endregion

        #region Properties
        public int ColumnsCount { get; private set; }
        public int RowsCount { get; private set; }

        public List<string> ColumnHeaders { get; set; }
        public string SelectedColumnHeader { get; set; }

        public string SearchTerm { get; set; }
        #endregion

        #region Methods
        protected override Task InitializeAsync()
        {
            _projectManager.ProjectActivationAsync += OnProjectActivationAsync;

            return base.InitializeAsync();
        }

        protected override Task CloseAsync()
        {
            _projectManager.ProjectActivationAsync -= OnProjectActivationAsync;

            return base.CloseAsync();
        }

        private async Task OnProjectActivationAsync(object sender, ProjectUpdatingCancelEventArgs e)
        {
            if (_csvTextEditorInstance is not null)
            {
                _csvTextEditorInstance.TextChanged -= OnTextChanged;
            }

            var project = e.NewProject as Project;

            _csvTextEditorInstance = _csvTextEditorInstanceProvider.GetInstance(project);
            _csvTextEditorInstance.TextChanged += OnTextChanged;

            UpdateStatistic();
        }

        protected override void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            if (e.HasPropertyChanged(nameof(SelectedColumnHeader)))
            {
                var allText = _csvTextEditorInstance.GetText();
                var indexOfNewLine = allText.IndexOf(Symbols.NewLineEnd);

                if (indexOfNewLine != -1)
                {
                    var firstLine = allText.Substring(0, indexOfNewLine);
                    var headers = firstLine.Split(Symbols.Comma).Select(x => x.Trim()).ToArray();

                    var selectedHeaderIndex = Array.IndexOf(headers, SelectedColumnHeader);
                    var startHeaderIndex = firstLine.IndexOf(SelectedColumnHeader);

                    _csvTextEditorInstance.GotoPosition(1, selectedHeaderIndex);
                    _csvTextEditorInstance.SetSelection(startHeaderIndex, SelectedColumnHeader.Length);
                }
            }
            else if (e.HasPropertyChanged(nameof(SearchTerm)))
            {
                _csvTextEditorInstance.SetSelectedText(SearchTerm);
            }

            base.OnPropertyChanged(e);
        }        

        private void UpdateColumnHeaders()
        {
            var allText = _csvTextEditorInstance.GetText();
            var indexOfNewLine = allText.IndexOf(Symbols.NewLineEnd);

            if (indexOfNewLine != -1)
            {
                var firstLine = allText.Substring(0, indexOfNewLine);
                ColumnHeaders = firstLine.Split(Symbols.Comma).Select(x => x.Trim()).OrderBy(x => x).ToList();
            }
            else
            {
                ColumnHeaders = new List<string>();
            }
        }

        private void UpdateStatistic()
        {
            RowsCount = _csvTextEditorInstance.LinesCount;
            ColumnsCount = _csvTextEditorInstance.ColumnsCount;
        }

        private void OnTextChanged(object sender, EventArgs e)
        {
            UpdateColumnHeaders();
            UpdateStatistic();
        }
        #endregion
    }
}
