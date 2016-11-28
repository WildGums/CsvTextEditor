// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CsvTextEditorControl.xaml.cs" company="WildGums">
//   Copyright (c) 2008 - 2016 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.CsvTextEditor
{
    using System;
    using System.Windows;
    using System.Windows.Input;
    using System.Xml;
    using Catel.IoC;
    using Catel.Logging;
    using ICSharpCode.AvalonEdit.Document;
    using ICSharpCode.AvalonEdit.Highlighting;
    using ICSharpCode.AvalonEdit.Highlighting.Xshd;
    using Services;

    public partial class CsvTextEditorControl
    {
        #region Fields
        private static readonly ILog Log = LogManager.GetCurrentClassLogger();

        private readonly IServiceLocator _serviceLocator;
        private readonly ITypeFactory _typeFactory;
        private ICsvTextEditorService _csvTextEditorService;

        private bool _isTextEditing;
        #endregion

        #region Constructors
        public CsvTextEditorControl()
        {
            InitializeComponent();

            _serviceLocator = this.GetServiceLocator();
            _typeFactory = _serviceLocator.ResolveType<ITypeFactory>();

            UpdateServiceRegistration();
        }
        #endregion

        #region Dependency properties
        public string Text
        {
            get { return (string) GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }

        public static readonly DependencyProperty TextProperty = DependencyProperty.Register(
            "Text", typeof (string), typeof (CsvTextEditorControl), new PropertyMetadata(default(string), (s, e) => ((CsvTextEditorControl) s).OnTextChanged()));

        public object Scope
        {
            get { return (object) GetValue(ScopeProperty); }
            set { SetValue(ScopeProperty, value); }
        }

        public static readonly DependencyProperty ScopeProperty = DependencyProperty.Register(
            "Scope", typeof (object), typeof (CsvTextEditorControl), new PropertyMetadata(default(object), (s, e) => ((CsvTextEditorControl) s).OnScopeChanged()));
        #endregion

        private void OnDeleteSelectedText(object sender, ExecutedRoutedEventArgs e)
        {
            _csvTextEditorService.DeleteSelectedText();
            Synchronize();
        }

        private void OnCut(object sender, ExecutedRoutedEventArgs e)
        {
            _csvTextEditorService.Cut();
            Synchronize();
        }

        private void OnPaste(object sender, ExecutedRoutedEventArgs e)
        {
            _csvTextEditorService.Paste();
            Synchronize();
        }

        private void OnUndo(object sender, ExecutedRoutedEventArgs e)
        {
            _csvTextEditorService.Undo();
            Synchronize();
        }

        private void OnRedo(object sender, ExecutedRoutedEventArgs e)
        {
            _csvTextEditorService.Redo();
            Synchronize();
        }

        private void OnDuplicateLine(object sender, ExecutedRoutedEventArgs e)
        {
            _csvTextEditorService.DuplicateLine();
            Synchronize();
        }

        private void OnAddColumn(object sender, ExecutedRoutedEventArgs e)
        {
            _csvTextEditorService.AddColumn();
            Synchronize();
        }

        private void OnRemoveLine(object sender, ExecutedRoutedEventArgs e)
        {
            _csvTextEditorService.RemoveLine();
            Synchronize();
        }

        private void OnAddLine(object sender, ExecutedRoutedEventArgs e)
        {
            _csvTextEditorService.AddLine();
            Synchronize();
        }

        private void OnRemoveColumn(object sender, ExecutedRoutedEventArgs e)
        {
            _csvTextEditorService.RemoveColumn();
            Synchronize();
        }

        private void OnGotoNextColumn(object sender, ExecutedRoutedEventArgs e)
        {
            _csvTextEditorService.GotoNextColumn();
        }

        private void OnGotoPreviousColumn(object sender, ExecutedRoutedEventArgs e)
        {
            _csvTextEditorService.GotoPreviousColumn();
        }

        private void OnTextChanged()
        {
            if (_isTextEditing)
            {
                return;
            }

            UpdateTextEditor();
        }

        private void OnScopeChanged()
        {
            UpdateServiceRegistration();
        }

        private void UpdateServiceRegistration()
        {
            if (_csvTextEditorService == null)
            {
                _csvTextEditorService = _typeFactory.CreateInstanceWithParametersAndAutoCompletion<CsvTextEditorService>(TextEditor);
            }

            _serviceLocator.RegisterInstance(_csvTextEditorService, Scope);
        }

        private void UpdateTextEditor()
        {
            Log.Info("UpdateTextEditor start");

            var document = TextEditor.Document;
            document.Changed -= OnTextDocumentChanged;

            var text = Text;

            _csvTextEditorService.Initialize(text);

            document.Changed += OnTextDocumentChanged;

            LoadSyntaxHighlighting();

            Log.Info("UpdateTextEditor end");
        }

        private void Synchronize()
        {
            SynchronizeDocumentText();
            LoadSyntaxHighlighting();
        }

        private void SynchronizeDocumentText()
        {
            Log.Info("SynchronizeDocumentText start");

            _isTextEditing = true;

            try
            {
                Text = TextEditor.Text;
            }
            finally
            {
                _isTextEditing = false;
            }

            Log.Info("SynchronizeDocumentText end");
        }

        private void LoadSyntaxHighlighting()
        {
            using (var s = typeof (CsvTextEditorControl).Assembly.GetManifestResourceStream("Orc.CsvTextEditor.Resources.Highlightings.CustomHighlighting.xshd"))
            {
                if (s == null)
                {
                    throw new InvalidOperationException("Could not find embedded resource");
                }

                using (var reader = new XmlTextReader(s))
                {
                    TextEditor.SyntaxHighlighting = HighlightingLoader.Load(reader, HighlightingManager.Instance);
                }
            }
        }

        private void OnTextDocumentChanged(object sender, DocumentChangeEventArgs e)
        {
            Log.Info("OnTextDocumentChanged start");

            _csvTextEditorService.RefreshLocation(e.Offset, e.InsertionLength - e.RemovalLength);

            Log.Info("OnTextDocumentChanged end");
        }
    }
}