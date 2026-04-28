namespace CsvTextEditor.ViewModels
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using Catel.MVVM;
    using Orc.CsvTextEditor;

    public class ProjectStatisticViewModel : ViewModelBase
    {
        private readonly ICsvTextEditorInstanceProvider _csvTextEditorInstanceProvider;
        private readonly ICsvTextEditorInstanceManager _csvTextEditorInstanceManager;

        private ICsvTextEditorInstance? _csvTextEditorInstance;
        private int _textChangesSubscribed = 0;

        public ProjectStatisticViewModel(ICsvTextEditorInstanceProvider csvTextEditorInstanceProvider, 
            IServiceProvider serviceProvider, ICsvTextEditorInstanceManager csvTextEditorInstanceManager)
            : base(serviceProvider)
        {
            _csvTextEditorInstanceProvider = csvTextEditorInstanceProvider;
            _csvTextEditorInstanceManager = csvTextEditorInstanceManager;
        }

        public int ColumnsCount { get; private set; }
        public int RowsCount { get; private set; }

        protected override async Task InitializeAsync()
        {
            await base.InitializeAsync();

            _csvTextEditorInstanceManager.InstanceRegistered += OnInstanceRegistered;

            var instance = _csvTextEditorInstanceManager.GetInstances().FirstOrDefault();
            if (instance is not null)
            {
                UpdateToNewInstance(instance);
            }
        }

        protected override async Task CloseAsync()
        {
            _csvTextEditorInstanceManager.InstanceRegistered -= OnInstanceRegistered;

            await base.CloseAsync();
        }

        private void OnInstanceRegistered(object? sender, CsvTextEditorEventArgs e)
        {
            UpdateToNewInstance(e.Instance);
        }

        private void UpdateToNewInstance(ICsvTextEditorInstance instance)
        {
            if (_csvTextEditorInstance is not null && _textChangesSubscribed > 0)
            {
                _csvTextEditorInstance.TextChanged -= OnTextChanged;
                _textChangesSubscribed--;
            }

            _csvTextEditorInstance = instance;
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
    }
}
