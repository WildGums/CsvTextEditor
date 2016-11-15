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
    using ICSharpCode.AvalonEdit;
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

            AvalonEditCommands.DeleteLine.InputGestures.Clear();
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

        public static readonly DependencyProperty DuplicateLineKeyGestureProperty = DependencyProperty.Register(
            "DuplicateLineKeyGesture", typeof (KeyGesture), typeof (CsvTextEditorControl), new PropertyMetadata(new KeyGesture(Key.D, ModifierKeys.Control)));

        public KeyGesture DuplicateLineKeyGesture
        {
            get { return (KeyGesture) GetValue(DuplicateLineKeyGestureProperty); }
            set { SetValue(DuplicateLineKeyGestureProperty, value); }
        }

        public static readonly DependencyProperty AddColumnKeyGestureProperty = DependencyProperty.Register(
            "AddColumnKeyGesture", typeof (KeyGesture), typeof (CsvTextEditorControl), new PropertyMetadata(new KeyGesture(Key.OemComma, ModifierKeys.None)));

        public KeyGesture AddColumnKeyGesture
        {
            get { return (KeyGesture) GetValue(AddColumnKeyGestureProperty); }
            set { SetValue(AddColumnKeyGestureProperty, value); }
        }

        public static readonly DependencyProperty RemoveLineKeyGestureProperty = DependencyProperty.Register(
            "RemoveLineKeyGesture", typeof (KeyGesture), typeof (CsvTextEditorControl), new PropertyMetadata(new KeyGesture(Key.L, ModifierKeys.Control)));

        public KeyGesture RemoveLineKeyGesture
        {
            get { return (KeyGesture) GetValue(RemoveLineKeyGestureProperty); }
            set { SetValue(RemoveLineKeyGestureProperty, value); }
        }

        public static readonly DependencyProperty AddLineKeyGestureProperty = DependencyProperty.Register(
            "AddLineKeyGesture", typeof (KeyGesture), typeof (CsvTextEditorControl), new PropertyMetadata(new KeyGesture(Key.Enter, ModifierKeys.None)));

        public KeyGesture AddLineKeyGesture
        {
            get { return (KeyGesture) GetValue(AddLineKeyGestureProperty); }
            set { SetValue(AddLineKeyGestureProperty, value); }
        }

        //
        public static readonly DependencyProperty RemoveColumnKeyGestureProperty = DependencyProperty.Register(
            "RemoveColumnKeyGesture", typeof (KeyGesture), typeof (CsvTextEditorControl), new PropertyMetadata(new KeyGesture(Key.OemComma, ModifierKeys.Control)));

        public KeyGesture RemoveColumnKeyGesture
        {
            get { return (KeyGesture) GetValue(RemoveColumnKeyGestureProperty); }
            set { SetValue(RemoveColumnKeyGestureProperty, value); }
        }

        public static readonly DependencyProperty GotoNextColumnKeyGestureProperty = DependencyProperty.Register(
            "GotoNextColumnKeyGesture", typeof (KeyGesture), typeof (CsvTextEditorControl), new PropertyMetadata(new KeyGesture(Key.Tab, ModifierKeys.None)));

        public KeyGesture GotoNextColumnKeyGesture
        {
            get { return (KeyGesture) GetValue(GotoNextColumnKeyGestureProperty); }
            set { SetValue(GotoNextColumnKeyGestureProperty, value); }
        }

        public static readonly DependencyProperty GotoPreviousColumnKeyGestureProperty = DependencyProperty.Register(
            "GotoPreviousColumnKeyGesture", typeof (KeyGesture), typeof (CsvTextEditorControl), new PropertyMetadata(new KeyGesture(Key.Tab, ModifierKeys.Control)));

        public KeyGesture GotoPreviousColumnKeyGesture
        {
            get { return (KeyGesture) GetValue(GotoPreviousColumnKeyGestureProperty); }
            set { SetValue(GotoPreviousColumnKeyGestureProperty, value); }
        }
        #endregion

        protected override void OnPreviewKeyDown(KeyEventArgs e)
        {
            base.OnPreviewKeyDown(e);

            if (IsKeyGestureMatches(DuplicateLineKeyGesture, e.Key))
            {
                OnDuplicateLine();

                e.Handled = true;
                return;
            }

            if (IsKeyGestureMatches(AddColumnKeyGesture, e.Key))
            {
                OnAddColumn();

                e.Handled = true;
                return;
            }

            if (IsKeyGestureMatches(RemoveLineKeyGesture, e.Key))
            {
                OnRemoveColumn();

                e.Handled = true;
                return;
            }

            if (IsKeyGestureMatches(AddLineKeyGesture, e.Key))
            {
                OnAddLine();

                e.Handled = true;
                return;
            }

            if (IsKeyGestureMatches(RemoveColumnKeyGesture, e.Key))
            {
                OnRemoveColumn();

                e.Handled = true;
                return;
            }

            if (IsKeyGestureMatches(GotoNextColumnKeyGesture, e.Key))
            {
                OnGotoNextColumn();

                e.Handled = true;
                return;
            }

            if (IsKeyGestureMatches(GotoPreviousColumnKeyGesture, e.Key))
            {
                OnGotoPreviousColumn();

                e.Handled = true;
                return;
            }
        }

        private void OnDuplicateLine()
        {
            _csvTextEditorService.DuplicateLine();
            Synchronize();
        }

        private void OnAddColumn()
        {
            _csvTextEditorService.AddColumn();
            Synchronize();
        }

        private void OnRemoveLine()
        {
            _csvTextEditorService.RemoveLine();
            Synchronize();
        }

        private void OnAddLine()
        {
            _csvTextEditorService.AddLine();
            Synchronize();
        }

        private void OnRemoveColumn()
        {
            _csvTextEditorService.RemoveColumn();
            Synchronize();
        }

        private void OnGotoNextColumn()
        {
            _csvTextEditorService.GotoNextColumn();
        }

        private void OnGotoPreviousColumn()
        {
            _csvTextEditorService.GotoPreviousColumn();
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

            _csvTextEditorService.UpdateTextLocation(e.Offset, e.InsertionLength - e.RemovalLength);

            Log.Info("OnTextDocumentChanged end");
        }

        private bool IsKeyGestureMatches(KeyGesture keyGesture, Key key)
        {
            return Keyboard.Modifiers == keyGesture.Modifiers && key == keyGesture.Key;
        }
    }
}