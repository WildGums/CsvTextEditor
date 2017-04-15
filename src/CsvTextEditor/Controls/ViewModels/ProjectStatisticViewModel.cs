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
    using Orc.CsvTextEditor.Services;

    public class ProjectStatisticViewModel : ViewModelBase
    {
        #region Fields
        private readonly IServiceLocator _serviceLocator;

        private ICsvTextEditorService _csvTextEditorService;
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
            if (e.ServiceType != typeof(ICsvTextEditorService))
            {
                return;
            }

            if (_csvTextEditorService != null)
            {
                _csvTextEditorService.TextChanged -= OnTextChanged;
            }

            _csvTextEditorService = _serviceLocator.ResolveType<ICsvTextEditorService>(e.Tag);
            _csvTextEditorService.TextChanged += OnTextChanged;

            UpdateStatistic();
        }

        private void UpdateStatistic()
        {
            RowsCount = _csvTextEditorService.LineCount;
            ColumnsCount = _csvTextEditorService.ColumnCount;
        }

        private void OnTextChanged(object sender, EventArgs e)
        {
            UpdateStatistic();
        }
        #endregion
    }
}