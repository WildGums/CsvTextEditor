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
    using ICSharpCode.AvalonEdit;
    using ICSharpCode.AvalonEdit.Document;
    using ICSharpCode.AvalonEdit.Editing;
    using ICSharpCode.AvalonEdit.Highlighting;
    using ICSharpCode.AvalonEdit.Highlighting.Xshd;
    using Services;

    public partial class CsvTextEditorControl
    {
        #region Fields
        private readonly BackgroundWorker _backgroundWorker;
        private readonly ColumnWidthCalculator _columnWidthCalculator;
        private readonly TabSpaceElementGenerator _elementGenerator;
        private readonly TextArea _textArea;
        private readonly TextDocument _textDocument;
        private readonly IPostprocessorProvider _postprocessorProvider;
        private readonly IServiceLocator _serviceLocator;
        private readonly ITypeFactory _typeFactory;

        private ICsvTextEditorService _csvTextEditorService;
        private bool _isTextEditing;
        private bool _isUpdating;
        #endregion

        #region Constructors
        public CsvTextEditorControl()
        {
            InitializeComponent();

            _columnWidthCalculator = new ColumnWidthCalculator();
            _elementGenerator = new TabSpaceElementGenerator(_columnWidthCalculator);
            _textArea = TextEditor.TextArea;
            _textDocument = TextEditor.Document;

            _textArea.TextView.ElementGenerators.Add(_elementGenerator);

            _backgroundWorker = new BackgroundWorker();
            _backgroundWorker.DoWork += OnBackgroundWorkerDoWork;

            _serviceLocator = this.GetServiceLocator();
            _postprocessorProvider = _serviceLocator.ResolveType<IPostprocessorProvider>();
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
            _textDocument.Changed -= OnTextDocumentChanged;
            _textDocument.UpdateFinished -= TextDocumentOnUpdateFinished;
            _textDocument.Changing -= TextDocumentOnChanging;

            var text = Text;

            if (ReferenceEquals(text, null))
            {
                return;
            }

            var lines = text.Split(new[] {Environment.NewLine}, StringSplitOptions.None);

            var columnWidthByLine =
                lines
                    .Select(x => x.Split(Symbols.Comma))
                    .Select(x => x.Select(y => y.Length + 1).ToArray())
                    .ToArray();

            var columnWidth = CalculateColumnWidth(columnWidthByLine);

            _elementGenerator.Lines = columnWidthByLine;
            _elementGenerator.ColumnWidth = columnWidth;

            TextEditor.Text = text;

            _textDocument.Changed += OnTextDocumentChanged;
            _textDocument.UpdateFinished += TextDocumentOnUpdateFinished;
            _textDocument.Changing += TextDocumentOnChanging;

            LoadSyntaxHighlighting();
        }

        private IPostprocessor _postprocessor;

        private void TextDocumentOnChanging(object sender, DocumentChangeEventArgs e)
        {
            var changeState = new DocumentChangingContext()
            {
                InsertedText = e.InsertedText.Text,
                Offset = e.Offset,
                ColumnWidthCalculator = _columnWidthCalculator,
                ElementGenerator = _elementGenerator,
                TextEditor = TextEditor,
                OldText = Text
            };

            _postprocessor = _postprocessorProvider.GetPostprocessors(TextEditor.Text, changeState);         
        }

        private void SynchronizeDocumentText()
        {
            _isTextEditing = true;

            try
            {
                Text = TextEditor.Text;
            }
            finally
            {
                _isTextEditing = false;
            }
        }

        private void TextDocumentOnUpdateFinished(object sender, EventArgs eventArgs)
        {
            if (ReferenceEquals(_postprocessor, null) || _isUpdating)
            {
                SynchronizeDocumentText();
                return;
            }            

            _isUpdating = true;

            // Note: important to remember this value, because can be changed
            var postprocessor = _postprocessor;

            postprocessor.Apply();

            SynchronizeDocumentText();
            UpdateTextEditor();

            postprocessor.RestoreCaret();

            _isUpdating = false;

            _postprocessor = null;
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

        private int[] CalculateColumnWidth(int[][] columnWidthByLine)
        {
            if (columnWidthByLine.Length == 0)
            {
                return new int[0];
            }

            var accum = new int[columnWidthByLine[0].Length];

            foreach (var line in columnWidthByLine)
            {
                if (line.Length > accum.Length)
                {
                    throw new ArgumentException("Records in CSV have to contain the same number of fields");
                }

                var length = Math.Min(accum.Length, line.Length);

                for (var i = 0; i < length; i++)
                {
                    accum[i] = Math.Max(accum[i], line[i]);
                }
            }

            return accum.ToArray();
        }

        private void OnTextDocumentChanged(object sender, DocumentChangeEventArgs e)
        {
            var affectedLocation = _textDocument.GetLocation(e.Offset);

            var columnWidth = _elementGenerator.ColumnWidth;
            var columnWidthByLine = _elementGenerator.Lines;

            var columnNumberWithOffset = _columnWidthCalculator.GetColumn(columnWidthByLine, affectedLocation);

            var myColumn = columnNumberWithOffset.ColumnNumber;
            var oldWidth = columnWidthByLine[affectedLocation.Line - 1][myColumn];
            var length = e.InsertionLength - e.RemovalLength;
            columnWidthByLine[affectedLocation.Line - 1][myColumn] = oldWidth + length;

            if (length > 0)
            {
                if (oldWidth + length > columnWidth[myColumn])
                {
                    columnWidth[columnNumberWithOffset.ColumnNumber] = oldWidth + length;

                    UpdateLines();
                }
            }
            else
            {
                var maxLength = columnWidthByLine.Where(x => x.Length > myColumn).Select(x => x[myColumn]).Max();

                if (maxLength != columnWidth[myColumn])
                {
                    columnWidth[myColumn] = maxLength;

                    UpdateLines();
                }
            }
        }

        private void UpdateLines()
        {
            _textArea.TextView.Redraw();

            if (!_backgroundWorker.IsBusy)
            {
                _backgroundWorker.RunWorkerAsync();
            }
        }

        private void OnBackgroundWorkerDoWork(object sender, DoWorkEventArgs e)
        {
            Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.SystemIdle, new Action(TextEditor.TextArea.TextView.Redraw));
        }
    }
}