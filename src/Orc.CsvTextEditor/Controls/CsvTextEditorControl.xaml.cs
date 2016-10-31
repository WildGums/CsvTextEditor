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
    using ICSharpCode.AvalonEdit;
    using ICSharpCode.AvalonEdit.Document;
    using ICSharpCode.AvalonEdit.Editing;
    using ICSharpCode.AvalonEdit.Highlighting;
    using ICSharpCode.AvalonEdit.Highlighting.Xshd;

    public partial class CsvTextEditorControl
    {
        private readonly ColumnWidthCalculator _columnWidthCalculator;
        private readonly BackgroundWorker _backgroundWorker;       
        private readonly TabSpaceElementGenerator _elementGenerator;
        private readonly TextArea _textArea; 
        private readonly TextDocument _textDocument; 

        #region Constructors
        public CsvTextEditorControl()
        {
            InitializeComponent();

            _columnWidthCalculator = new ColumnWidthCalculator();
            _elementGenerator = new TabSpaceElementGenerator(_columnWidthCalculator);
            _textArea = TextEditor.TextArea;
            _textDocument = TextEditor.Document;

            _textArea.TextView.ElementGenerators.Add(_elementGenerator);

            _backgroundWorker = (BackgroundWorker)FindResource("backgroundWoker");
        }
        #endregion

        public bool UpdateAllLinesInBackground { get; set; }

        public string Text
        {
            get { return (string) GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }

        public static readonly DependencyProperty TextProperty = DependencyProperty.Register(
            "Text", typeof(string), typeof(CsvTextEditorControl), new PropertyMetadata(default(string), (s, e) => ((CsvTextEditorControl)s).OnTextChanged()));

        private void OnTextChanged()
        {
            if (_isTextEditing)
            {
                return;
            }

            UpdateTextEditor();
        }

        private void UpdateTextEditor()
        {
            _textDocument.Changed -= OnDocumentChanged;

            var text = Text;

            if(ReferenceEquals(text, null))
            {
                return;
            }

            var lines = text.Split(new[] { Environment.NewLine }, StringSplitOptions.None);

            var columnWidthByLine =
                lines
                    .Select(x => x.Split(','))
                    .Select(x => x.Select(y => y.Length + 1).ToArray())
                    .ToArray();

            var columnWidth = CalculateColumnWidth(columnWidthByLine);

            _elementGenerator.Lines = columnWidthByLine;
            _elementGenerator.ColumnWidth = columnWidth;

            TextEditor.Text = text;

            _textDocument.Changed += OnDocumentChanged;

            LoadSyntaxHighlighting();
            AssignShortcutKeys();
        }

        private void AssignShortcutKeys()
        {
            AvalonEditCommands.DeleteLine.InputGestures.Add(new KeyGesture(Key.L, ModifierKeys.Control));
        }

        private void LoadSyntaxHighlighting()
        {
            using (var s = typeof(CsvTextEditorControl).Assembly.GetManifestResourceStream("Orc.CsvTextEditor.Resources.Highlightings.CustomHighlighting.xshd"))
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
                return new int[0];

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

        private bool _isTextEditing;
        private void OnDocumentChanged(object sender, DocumentChangeEventArgs e)
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

        private void UpdateLines()
        {
            if (!UpdateAllLinesInBackground)
            {
                _textArea.TextView.Redraw();
            }

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