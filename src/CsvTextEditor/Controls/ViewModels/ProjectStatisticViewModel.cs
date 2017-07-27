// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ProjectStatisticViewModel.cs" company="WildGums">
//   Copyright (c) 2008 - 2017 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace CsvTextEditor.ViewModels
{
    using System;
    using Catel;
    using Catel.IoC;
    using Catel.MVVM;
    using Orc.CsvTextEditor;

    public class ProjectStatisticViewModel : ViewModelBase
    {
        #region Fields
        private readonly IServiceLocator _serviceLocator;

        private ICsvTextEditorInstance _csvTextEditorInstance;
        #endregion

        #region Constructors
        public ProjectStatisticViewModel(IServiceLocator serviceLocator)
        {
            Argument.IsNotNull(() => serviceLocator);

            _serviceLocator = serviceLocator;

            _serviceLocator.TypeRegistered += OnTypeRegistered;
        }
        #endregion

        #region Properties
        public int ColumnsCount { get; private set; }
        public int RowsCount { get; private set; }
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

        private void UpdateStatistic()
        {
            RowsCount = _csvTextEditorInstance.LinesCount;
            ColumnsCount = _csvTextEditorInstance.ColumnsCount;
        }

        private void OnTextChanged(object sender, EventArgs e)
        {
            UpdateStatistic();
        }
        #endregion
    }
}