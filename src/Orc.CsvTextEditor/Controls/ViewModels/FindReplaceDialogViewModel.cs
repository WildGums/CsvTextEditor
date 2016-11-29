// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FindReplaceDialogViewModel.cs" company="WildGums">
//   Copyright (c) 2008 - 2016 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.CsvTextEditor
{
    using System.Media;
    using Catel.IoC;
    using Catel.MVVM;
    using Controls;
    using Services;

    public class FindReplaceDialogViewModel : ViewModelBase
    {
        #region Fields
        private readonly ICsvTextEditorSearchService _csvTextEditorSearchService;
        private readonly ICsvTextEditorService _csvTextEditorService;
        #endregion

        #region Constructors
        public FindReplaceDialogViewModel(object scope)
        {
            var serviceLocator = this.GetServiceLocator();

            if (_csvTextEditorSearchService == null && serviceLocator.IsTypeRegistered<ICsvTextEditorSearchService>(scope))
            {
                _csvTextEditorSearchService = serviceLocator.ResolveType<ICsvTextEditorSearchService>(scope);
            }

            if (_csvTextEditorService == null && serviceLocator.IsTypeRegistered<ICsvTextEditorService>(scope))
            {
                _csvTextEditorService = serviceLocator.ResolveType<ICsvTextEditorService>(scope);
            }

            FindNext = new Command<string>(OnFindNext);
            Replace = new Command<object>(OnReplace);
            ReplaceAll = new Command<object>(OnReplaceAll);

            FindReplaceSettings = new FindReplaceSettings();
        }
        #endregion

        #region Properties
        public override string Title => "Find and Replace";

        [Model]
        public FindReplaceSettings FindReplaceSettings { get; set; }

        public Command<string> FindNext { get; private set; }
        public Command<object> Replace { get; private set; }
        public Command<object> ReplaceAll { get; private set; }
        #endregion

        private void OnReplaceAll(object parameter)
        {
            var values = (object[]) parameter;
            var textToFind = values[0] as string ?? string.Empty;
            var replacementText = values[1] as string ?? string.Empty;

            _csvTextEditorSearchService.ReplaceAll(textToFind, replacementText, FindReplaceSettings);

            _csvTextEditorService.RefreshView();
        }

        private void OnReplace(object parameter)
        {
            var values = (object[]) parameter;
            var textToFind = values[0] as string ?? string.Empty;
            var replacementText = values[1] as string ?? string.Empty;

            if (!_csvTextEditorSearchService.Replace(textToFind, replacementText, FindReplaceSettings))
            {
                SystemSounds.Beep.Play();
            }

            _csvTextEditorService.RefreshView();
        }

        private void OnFindNext(string text)
        {
            var textToFind = text ?? string.Empty;

            if (!_csvTextEditorSearchService.FindNext(textToFind, FindReplaceSettings))
            {
                SystemSounds.Beep.Play();
            }
        }
    }
}