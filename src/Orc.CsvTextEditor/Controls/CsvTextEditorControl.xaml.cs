// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CsvTextEditorControl.xaml.cs" company="WildGums">
//   Copyright (c) 2008 - 2016 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.CsvTextEditor
{
    using System;
    using System.ComponentModel;
    using System.Linq;
    using System.Windows;
    using System.Windows.Input;
    using System.Xml;
    using Catel.IoC;
    using Catel.Logging;
    using Catel.Threading;
    using ICSharpCode.AvalonEdit.Document;
    using ICSharpCode.AvalonEdit.Editing;
    using ICSharpCode.AvalonEdit.Highlighting;
    using ICSharpCode.AvalonEdit.Highlighting.Xshd;
    using Services;

    public partial class CsvTextEditorControl
    {
        private static readonly ILog Log = LogManager.GetCurrentClassLogger();

        #region Fields
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
            "Text", typeof(string), typeof(CsvTextEditorControl), new PropertyMetadata(default(string), (s, e) => ((CsvTextEditorControl)s).OnTextChanged()));        

        public object Scope
        {
            get { return (object) GetValue(ScopeProperty); }
            set { SetValue(ScopeProperty, value); }
        }

        public static readonly DependencyProperty ScopeProperty = DependencyProperty.Register(
            "Scope", typeof(object), typeof(CsvTextEditorControl), new PropertyMetadata(default(object), (s, e) => ((CsvTextEditorControl)s).OnScopeChanged()));
        #endregion

        protected override void OnPreviewKeyDown(KeyEventArgs e)
        {
            base.OnPreviewKeyDown(e);

           // var offset = TextEditor.CaretOffset;

            Log.Info("Add column");

            if (Keyboard.Modifiers == ModifierKeys.Control)
            {
                if (e.Key == Key.OemComma)
                {
                    _csvTextEditorService.RemoveColumn();
                    e.Handled = true;
                }

                if (e.Key == Key.D)
                {
                    _csvTextEditorService.DuplicateLine();
                    e.Handled = true;
                }
            }
            
            if (!e.Handled && e.Key == Key.OemComma)
            {
                _csvTextEditorService.AddColumn();
                e.Handled = true;
            }

            if (!e.Handled && e.Key == Key.Enter)
            {
                _csvTextEditorService.AddLine();
                e.Handled = true;
            }
            
            if (e.Handled)
            {
                SynchronizeDocumentText();
                LoadSyntaxHighlighting();
            }

            Log.Info("Added column");
        }

        private void OnTextChanged()
        {
            if (_isTextEditing)
            {
                return;
            }

            Log.Info("text changed");

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
            _csvTextEditorService.UpdateText(text);

            document.Changed += OnTextDocumentChanged;

            LoadSyntaxHighlighting();

            Log.Info("UpdateTextEditor end");
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

            _csvTextEditorService.UpdateTextLocation(e.Offset, e.InsertionLength - e.RemovalLength);

            Log.Info("OnTextDocumentChanged end");
        }
    }
}