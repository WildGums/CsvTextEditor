// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FindReplaceTextEditorTool.cs" company="WildGums">
//   Copyright (c) 2008 - 2017 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.CsvTextEditor
{
    using System.Threading.Tasks;
    using Catel;
    using Catel.IoC;
    using Catel.MVVM;
    using Catel.Services;
    using Catel.Threading;
    using ICSharpCode.AvalonEdit;
    using Services;

    public class FindReplaceTextEditorTool : CsvTextEditorToolBase
    {
        #region Fields
        private readonly ICsvTextEditorFindReplaceSerivce _csvTextEditorFindReplaceSerivce;
        private readonly IUIVisualizerService _uiVisualizerService;

        private FindReplaceDialogViewModel _findReplaceViewModel;
        #endregion

        #region Constructors
        public FindReplaceTextEditorTool(TextEditor textEditor, ICsvTextEditorService csvTextEditorService,
            IUIVisualizerService uiVisualizerService, ITypeFactory typeFactory)
            : base(textEditor, csvTextEditorService)
        {
            Argument.IsNotNull(() => uiVisualizerService);
            Argument.IsNotNull(() => typeFactory);

            _uiVisualizerService = uiVisualizerService;

            _csvTextEditorFindReplaceSerivce = typeFactory.CreateInstanceWithParametersAndAutoCompletion<CsvTextEditorFindReplaceService>(TextEditor);
        }
        #endregion

        #region Properties
        public override string Name => "CsvTextEditor.FindReplaceTextEditorTool";
        #endregion

        protected override void OnOpen()
        {
            _findReplaceViewModel = new FindReplaceDialogViewModel(_csvTextEditorFindReplaceSerivce, CsvTextEditorService);

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
        
        private Task OnClosedAsync(object sender, ViewModelClosedEventArgs args)
        {
            return TaskHelper.RunAndWaitAsync(() => RaiseClosedEvent());
        }
    }
}