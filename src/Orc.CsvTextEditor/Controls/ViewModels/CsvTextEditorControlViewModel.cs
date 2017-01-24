// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CsvTextEditorControlViewModel.cs" company="WildGums">
//   Copyright (c) 2008 - 2017 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.CsvTextEditor
{
    using Catel;
    using Catel.IoC;
    using Catel.Logging;
    using Catel.MVVM;
    using Services;

    internal class CsvTextEditorControlViewModel : ViewModelBase
    {
        #region Fields
        private static readonly ILog Log = LogManager.GetCurrentClassLogger();

        private readonly IServiceLocator _serviceLocator;
        private ICsvTextEditorService _csvTextEditorService;
        private ICsvTextSynchronizationService _csvTextSynchronizationService;
        #endregion

        #region Constructors
        public CsvTextEditorControlViewModel(IServiceLocator serviceLocator)
        {
            _serviceLocator = serviceLocator;
            Argument.IsNotNull(() => serviceLocator);

            Paste = new Command(() => _csvTextEditorService.Paste());
            Cut = new Command(() => _csvTextEditorService.Cut());

            GotoNextColumn = new Command(() => _csvTextEditorService.GotoNextColumn());
            GotoPreviousColumn = new Command(() => _csvTextEditorService.GotoPreviousColumn());

            Undo = new Command(() => _csvTextEditorService.Undo(), () => _csvTextEditorService.CanUndo);
            Redo = new Command(() => _csvTextEditorService.Redo(), () => _csvTextEditorService.CanRedo);

            AddLine = new Command(() => _csvTextEditorService.AddLine());
            RemoveLine = new Command(() => _csvTextEditorService.RemoveLine());
            DuplicateLine = new Command(() => _csvTextEditorService.DuplicateLine());
            RemoveColumn = new Command(() => _csvTextEditorService.RemoveColumn());
            AddColumn = new Command(() => _csvTextEditorService.AddColumn());
            DeleteNextSelectedText = new Command(() => _csvTextEditorService.DeleteNextSelectedText());
            DeletePreviousSelectedText = new Command(() => _csvTextEditorService.DeletePreviousSelectedText());
        }
        #endregion

        #region Properties
        public object Scope { get; set; }
        public string Text { get; set; }

        public Command Paste { get; set; }
        public Command Cut { get; set; }

        public Command GotoNextColumn { get; }
        public Command GotoPreviousColumn { get; }
        public Command Redo { get; }
        public Command Undo { get; }
        public Command ShowFindReplaceDialog { get; }
        public Command DeletePreviousSelectedText { get; }
        public Command DeleteNextSelectedText { get; }
        public Command AddColumn { get; }
        public Command RemoveColumn { get; }
        public Command DuplicateLine { get; }
        public Command RemoveLine { get; }
        public Command AddLine { get; }
        #endregion

        private void OnTextChanged()
        {
            if (_csvTextSynchronizationService?.IsSynchronizing ?? true)
            {
                return;
            }

            using (_csvTextSynchronizationService.SynchronizeInScope())
            {
                _csvTextEditorService.Initialize(Text);
            }
        }

        private void OnScopeChanged()
        {
            var scope = Scope;

            if (_csvTextEditorService == null && _serviceLocator.IsTypeRegistered<ICsvTextEditorService>(scope))
            {
                _csvTextEditorService = _serviceLocator.ResolveType<ICsvTextEditorService>(scope);
            }

            if (_csvTextSynchronizationService == null && _serviceLocator.IsTypeRegistered<ICsvTextSynchronizationService>(scope))
            {
                _csvTextSynchronizationService = _serviceLocator.ResolveType<ICsvTextSynchronizationService>(scope);
            }
        }
    }
}