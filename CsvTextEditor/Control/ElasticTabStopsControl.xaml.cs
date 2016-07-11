using System;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Windows.Input;
using System.Xml;
using ICSharpCode.AvalonEdit;
using ICSharpCode.AvalonEdit.Document;
using ICSharpCode.AvalonEdit.Highlighting;
using ICSharpCode.AvalonEdit.Highlighting.Xshd;

namespace CsvTextEditor.Control
{
    /// <summary>
    /// Interaction logic for ElasticTabStopsControl.xaml
    /// </summary>
    public partial class CsvTextEditorControl : TextEditor
    {
        readonly TabSpaceElementGenerator _elementGenerator;
        readonly BackgroundWorker _backgroundWorker;
        private readonly ColumnWidthCalculator _columnWidthCalculator;

        public CsvTextEditorControl()
        {
            InitializeComponent();

            _columnWidthCalculator = new ColumnWidthCalculator();
            _elementGenerator = new TabSpaceElementGenerator(_columnWidthCalculator);
            TextArea.TextView.ElementGenerators.Add(_elementGenerator);

            _backgroundWorker = (BackgroundWorker)FindResource("backgroundWoker");
        }

        public bool UpdateAllLinesInBackground { get; set; }

        public void Initialize(string[] lines)
        {
            Document.Changed -= OnDocumentChanging;

            var columnWidthByLine =
                lines
                    .Select(x => x.Split(','))
                    .Select(x => x.Select(y => y.Length + 1).ToArray())
                    .ToArray();

            var columnWidth = CalculateColumnWidth(columnWidthByLine);

            _elementGenerator.Lines = columnWidthByLine;
            _elementGenerator.ColumnWidth = columnWidth;

            Text = string.Join(Environment.NewLine, lines);
            Document.Changed += OnDocumentChanging;

            LoadSyntaxHighlighting();
            AssignShortcutKeys();
        }

        private void AssignShortcutKeys()
        {
            AvalonEditCommands.DeleteLine.InputGestures.Add(new KeyGesture(Key.L, ModifierKeys.Control));
        }

        private void LoadSyntaxHighlighting()
        {
            using (var s = typeof(MainWindow).Assembly.GetManifestResourceStream("CsvTextEditor.Control.CustomHighlighting.xshd"))
            {
                if (s == null)
                {
                    throw new InvalidOperationException("Could not find embedded resource");
                }

                using (var reader = new XmlTextReader(s))
                {
                    SyntaxHighlighting = HighlightingLoader.Load(reader, HighlightingManager.Instance);
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
                if (line.Length != accum.Length)
                    throw new ArgumentException("Records in CSV have to contain the same number of fields");

                for (int i = 0; i < accum.Length; i++)
                {
                    accum[i] = Math.Max(accum[i], line[i]);
                }
            }

            return accum.ToArray();
        }

        private void OnDocumentChanging(object sender, DocumentChangeEventArgs e)
        {
            var affectedLocation = Document.GetLocation(e.Offset);

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
                var maxLength = columnWidthByLine.Select(x => x[myColumn]).Max();

                if (maxLength != columnWidth[myColumn])
                {
                    columnWidth[myColumn] = maxLength;

                    UpdateLines();
                }
            }
        }

        private void UpdateLines()
        {
            if (!UpdateAllLinesInBackground) TextArea.TextView.Redraw();

            if (!_backgroundWorker.IsBusy) _backgroundWorker.RunWorkerAsync();
        }

        private void BackgroundWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.SystemIdle, new Action(TextArea.TextView.Redraw));
        }
    }
}
