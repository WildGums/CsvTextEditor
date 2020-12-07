// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FindViewModel.cs" company="WildGums">
//   Copyright (c) 2008 - 2017 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace CsvTextEditor.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Linq;
    using Catel;
    using Catel.Collections;
    using Catel.Data;
    using Catel.IoC;
    using Catel.Linq;
    using Catel.MVVM;
    using CsvTextEditor.Models;
    using Orc.CsvTextEditor;

    public class FindViewModel : ViewModelBase
    {
        #region Fields
        private readonly IServiceLocator _serviceLocator;
        private readonly ICsvTextEditorInstanceProvider _csvTextEditorInstanceProvider;
        
        private ICsvTextEditorInstance _csvTextEditorInstance;
        private int _textChangedSubscribed = 0;
        #endregion

        #region Constructors
        public FindViewModel(IServiceLocator serviceLocator, ICsvTextEditorInstanceProvider csvTextEditorInstanceProvider)
        {
            Argument.IsNotNull(() => serviceLocator);
            Argument.IsNotNull(() => csvTextEditorInstanceProvider);

            _serviceLocator = serviceLocator;
            _csvTextEditorInstanceProvider = csvTextEditorInstanceProvider;

            _serviceLocator.TypeRegistered += OnTypeRegistered;
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

        protected override void OnPropertyChanged(AdvancedPropertyChangedEventArgs e)
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

        private void OnTypeRegistered(object sender, TypeRegisteredEventArgs e)
        {
            if (e.ServiceType != typeof(ICsvTextEditorInstance))
            {
                return;
            }

            if (_csvTextEditorInstance != null && _textChangedSubscribed > 0)
            {
                _csvTextEditorInstance.TextChanged -= OnTextChanged;
                _textChangedSubscribed--;
            }

            _csvTextEditorInstance = _csvTextEditorInstanceProvider.GetInstance((Project)e.Tag);
            _csvTextEditorInstance.TextChanged += OnTextChanged;
            _textChangedSubscribed++;

            UpdateStatistic();
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
