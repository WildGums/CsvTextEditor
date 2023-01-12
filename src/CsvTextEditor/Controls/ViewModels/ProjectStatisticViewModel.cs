namespace CsvTextEditor.ViewModels
{
    using System;
    using Catel;
    using Catel.IoC;
    using Catel.MVVM;
    using CsvTextEditor.Models;
    using Orc.CsvTextEditor;

    public class ProjectStatisticViewModel : ViewModelBase
    {
        #region Fields
        private readonly IServiceLocator _serviceLocator;
        private readonly ICsvTextEditorInstanceProvider _csvTextEditorInstanceProvider;
        
        private ICsvTextEditorInstance _csvTextEditorInstance;
        private int _textChangesSubscribed = 0;
        #endregion

        #region Constructors
        public ProjectStatisticViewModel(IServiceLocator serviceLocator, ICsvTextEditorInstanceProvider csvTextEditorInstanceProvider)
        {
            ArgumentNullException.ThrowIfNull(serviceLocator);
            ArgumentNullException.ThrowIfNull(csvTextEditorInstanceProvider);

            _serviceLocator = serviceLocator;
            _csvTextEditorInstanceProvider = csvTextEditorInstanceProvider;

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

            if (_csvTextEditorInstance is not null && _textChangesSubscribed > 0)
            {
                _csvTextEditorInstance.TextChanged -= OnTextChanged;
                _textChangesSubscribed--;
            }

            _csvTextEditorInstance = _csvTextEditorInstanceProvider.GetInstance((Project)e.Tag);
            _csvTextEditorInstance.TextChanged += OnTextChanged;
            _textChangesSubscribed++;

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
