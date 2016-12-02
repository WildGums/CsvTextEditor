// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FindReplaceTextEditorTool.cs" company="WildGums">
//   Copyright (c) 2008 - 2016 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.CsvTextEditor.CsvTextEditorToolManagement
{
    using System.Threading.Tasks;
    using Catel;
    using Catel.IoC;
    using Catel.MVVM;
    using Catel.Services;
    using Catel.Threading;
    using Services;

    public class FindReplaceTextEditorTool : CsvTextEditorToolBase
    {
        #region Fields
        private readonly IServiceLocator _serviceLocator;
        private readonly ITypeFactory _typeFactory;
        private readonly IUIVisualizerService _uiVisualizerService;
        private ICsvTextEditorFindReplaceSerivce _csvTextEditorFindReplaceSerivce;
        private ICsvTextEditorService _csvTextEditorService;

        private FindReplaceDialogViewModel _findReplaceViewModel;
        #endregion

        #region Constructors
        public FindReplaceTextEditorTool(IUIVisualizerService uiVisualizerService,
            IServiceLocator serviceLocator, ITypeFactory typeFactory)
        {
            Argument.IsNotNull(() => uiVisualizerService);
            Argument.IsNotNull(() => serviceLocator);
            Argument.IsNotNull(() => typeFactory);

            _uiVisualizerService = uiVisualizerService;
            _serviceLocator = serviceLocator;
            _typeFactory = typeFactory;
        }
        #endregion

        public override void Open()
        {
            _findReplaceViewModel = new FindReplaceDialogViewModel(_csvTextEditorFindReplaceSerivce, _csvTextEditorService);

            _uiVisualizerService.ShowAsync(_findReplaceViewModel);

            _findReplaceViewModel.ClosedAsync += OnClosedAsync;
        }

        public override void Close()
        {
            if (_findReplaceViewModel == null)
            {
                return;
            }

            _findReplaceViewModel.CloseViewModelAsync(null).RunSynchronously();
            _findReplaceViewModel.ClosedAsync -= OnClosedAsync;
        }

        public override void OnInitialize(object scope = null)
        {
            if (!_serviceLocator.IsTypeRegistered<ICsvTextEditorFindReplaceSerivce>(scope))
            {
                var textEditor = TexEditor;

                _csvTextEditorFindReplaceSerivce = _typeFactory.CreateInstanceWithParametersAndAutoCompletion<CsvTextEditorFindReplaceSerivce>(textEditor);
                _serviceLocator.RegisterInstance(_csvTextEditorFindReplaceSerivce, scope);
            }
            else
            {
                _csvTextEditorFindReplaceSerivce = _serviceLocator.ResolveType<ICsvTextEditorFindReplaceSerivce>(scope);
            }

            _csvTextEditorService = _serviceLocator.ResolveType<ICsvTextEditorService>(scope);
        }

        private Task OnClosedAsync(object sender, ViewModelClosedEventArgs args)
        {
            return TaskHelper.RunAndWaitAsync(() => RaiseClosedEvent());
        }
    }
}