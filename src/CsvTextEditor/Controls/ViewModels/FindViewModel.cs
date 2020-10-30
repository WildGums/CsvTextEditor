// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FindViewModel.cs" company="WildGums">
//   Copyright (c) 2008 - 2017 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace CsvTextEditor.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Catel;
    using Catel.IoC;
    using Catel.Linq;
    using Catel.MVVM;
    using Orc.CsvTextEditor;

    public class FindViewModel : ViewModelBase
    {
        #region Fields
        private readonly IServiceLocator _serviceLocator;

        private ICsvTextEditorInstance _csvTextEditorInstance;
        #endregion

        #region Constructors
        public FindViewModel(IServiceLocator serviceLocator)
        {
            Argument.IsNotNull(() => serviceLocator);

            _serviceLocator = serviceLocator;

            _serviceLocator.TypeRegistered += OnTypeRegistered;
        }
        #endregion

        #region Properties
        public int ColumnsCount { get; private set; }
        public int RowsCount { get; private set; }

        public List<string> ColumnHeaders { get; set; }
        public string SelectedColumnHeader { get; set; }
        #endregion

        #region Methods
        private void OnTypeRegistered(object sender, TypeRegisteredEventArgs e)
        {
            if (e.ServiceType != typeof(ICsvTextEditorInstance))
            {
                return;
            }

            if (_csvTextEditorInstance != null)
            {
                _csvTextEditorInstance.TextChanged -= OnTextChanged;
            }

            _csvTextEditorInstance = _serviceLocator.ResolveType<ICsvTextEditorInstance>(e.Tag);
            _csvTextEditorInstance.TextChanged += OnTextChanged;

            UpdateStatistic();
        }

        private void UpdateColumnHeaders()
        {
            var allText = _csvTextEditorInstance.GetText();
            var indexOfNewLine = allText.IndexOf(Symbols.NewLineEnd);

            if (indexOfNewLine != -1)
            {
                var firstLine = allText.Substring(0, indexOfNewLine);
                ColumnHeaders = firstLine.Split(Symbols.Comma).Select(x => x).ToList();
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
