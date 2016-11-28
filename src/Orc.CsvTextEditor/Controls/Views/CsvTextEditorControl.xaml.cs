// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CsvTextEditorControl.xaml.cs" company="WildGums">
//   Copyright (c) 2008 - 2016 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.CsvTextEditor
{
    using System.Data;
    using System.Windows;
    using System.Windows.Input;
    using Catel.IoC;
    using Catel.Logging;
    using Catel.MVVM.Views;
    using Services;

    public partial class CsvTextEditorControl
    {
        #region Fields
        private static readonly ILog Log = LogManager.GetCurrentClassLogger();
        #endregion

        #region Constructors
        static CsvTextEditorControl()
        {
            typeof (CsvTextEditorControl).AutoDetectViewPropertiesToSubscribe();
        }

        public CsvTextEditorControl()
        {
            InitializeComponent();
        }
        #endregion

        #region Dependency properties
        [ViewToViewModel(MappingType = ViewToViewModelMappingType.TwoWayViewWins)]
        public string Text
        {
            get { return (string) GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }

        public static readonly DependencyProperty TextProperty = DependencyProperty.Register(
            "Text", typeof (string), typeof (CsvTextEditorControl), new PropertyMetadata(default(string)));

        [ViewToViewModel(MappingType = ViewToViewModelMappingType.ViewToViewModel)]
        public object Scope
        {
            get { return (object) GetValue(ScopeProperty); }
            set { SetValue(ScopeProperty, value); }
        }

        public static readonly DependencyProperty ScopeProperty = DependencyProperty.Register(
            "Scope", typeof (object), typeof (CsvTextEditorControl), new PropertyMetadata(default(object)));
        #endregion

        //private void OnShowFindReplaceDialog(object sender, ExecutedRoutedEventArgs e)
        //{
        //    _csvTextEditorService.ShowFindReplaceDialog();
        //}

        //private void OnDeleteForward(object sender, ExecutedRoutedEventArgs e)
        //{
        //    _csvTextEditorService.DeleteNextSelectedText();
        //    Synchronize();
        //}

        //private void OnDeleteBack(object sender, ExecutedRoutedEventArgs e)
        //{
        //    _csvTextEditorService.DeletePreviousSelectedText();
        //    Synchronize();
        //}

        //private void OnCut(object sender, ExecutedRoutedEventArgs e)
        //{
        //    _csvTextEditorService.Cut();
        //    Synchronize();
        //}

        //private void OnPaste(object sender, ExecutedRoutedEventArgs e)
        //{
        //    _csvTextEditorService.Paste();
        //    Synchronize();
        //}

        //private void OnUndo(object sender, ExecutedRoutedEventArgs e)
        //{
        //    _csvTextEditorService.Undo();
        //    Synchronize();
        //}

        //private void OnRedo(object sender, ExecutedRoutedEventArgs e)
        //{
        //    _csvTextEditorService.Redo();
        //    Synchronize();
        //}

        //private void OnDuplicateLine(object sender, ExecutedRoutedEventArgs e)
        //{
        //    _csvTextEditorService.DuplicateLine();
        //    Synchronize();
        //}

        //private void OnAddColumn(object sender, ExecutedRoutedEventArgs e)
        //{
        //    _csvTextEditorService.AddColumn();
        //    Synchronize();
        //}

        //private void OnRemoveLine(object sender, ExecutedRoutedEventArgs e)
        //{
        //    _csvTextEditorService.RemoveLine();
        //    Synchronize();
        //}

        //private void OnAddLine(object sender, ExecutedRoutedEventArgs e)
        //{
        //    _csvTextEditorService.AddLine();
        //    Synchronize();
        //}

        //private void OnRemoveColumn(object sender, ExecutedRoutedEventArgs e)
        //{
        //    _csvTextEditorService.RemoveColumn();
        //    Synchronize();
        //}

        //private void OnGotoNextColumn(object sender, ExecutedRoutedEventArgs e)
        //{
        //    _csvTextEditorService.GotoNextColumn();
        //}

        //private void OnGotoPreviousColumn(object sender, ExecutedRoutedEventArgs e)
        //{
        //    _csvTextEditorService.GotoPreviousColumn();
        //}
    }
}