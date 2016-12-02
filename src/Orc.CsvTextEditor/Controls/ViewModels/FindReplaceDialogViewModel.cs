// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FindReplaceDialogViewModel.cs" company="WildGums">
//   Copyright (c) 2008 - 2016 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.CsvTextEditor
{
    using System.Media;
    using Catel;
    using Catel.IoC;
    using Catel.MVVM;
    using Controls;
    using Services;

    internal class FindReplaceDialogViewModel : ViewModelBase
    {
        #region Fields
        private readonly ICsvTextEditorFindReplaceSerivce _csvTextEditorFindReplaceSerivce;
        private readonly ICsvTextEditorService _csvTextEditorService;
        #endregion

        #region Constructors
        public FindReplaceDialogViewModel(ICsvTextEditorFindReplaceSerivce csvTextEditorFindReplaceSerivce, ICsvTextEditorService csvTextEditorService)
        {
            Argument.IsNotNull(() => csvTextEditorFindReplaceSerivce);
            Argument.IsNotNull(() => csvTextEditorService);

            _csvTextEditorFindReplaceSerivce = csvTextEditorFindReplaceSerivce;
            _csvTextEditorService = csvTextEditorService;

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

            _csvTextEditorFindReplaceSerivce.ReplaceAll(textToFind, replacementText, FindReplaceSettings);

            _csvTextEditorService.RefreshView();
        }

        private void OnReplace(object parameter)
        {
            var values = (object[]) parameter;
            var textToFind = values[0] as string ?? string.Empty;
            var replacementText = values[1] as string ?? string.Empty;

            if (!_csvTextEditorFindReplaceSerivce.Replace(textToFind, replacementText, FindReplaceSettings))
            {
                SystemSounds.Beep.Play();
            }

            _csvTextEditorService.RefreshView();
        }

        private void OnFindNext(string text)
        {
            var textToFind = text ?? string.Empty;

            if (!_csvTextEditorFindReplaceSerivce.FindNext(textToFind, FindReplaceSettings))
            {
                SystemSounds.Beep.Play();
            }
        }
    }
}