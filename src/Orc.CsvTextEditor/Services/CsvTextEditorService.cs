// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CsvTextEditorService.cs" company="WildGums">
//   Copyright (c) 2008 - 2016 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.CsvTextEditor.Services
{
    using System;
    using System.Windows;
    using System.Xml;
    using Catel;
    using Catel.IoC;
    using Catel.MVVM;
    using Catel.Services;
    using ICSharpCode.AvalonEdit;
    using ICSharpCode.AvalonEdit.Document;
    using ICSharpCode.AvalonEdit.Highlighting;
    using ICSharpCode.AvalonEdit.Highlighting.Xshd;
    using Transformers;

    internal class CsvTextEditorService : ICsvTextEditorService
    {
        #region Fields
        private readonly ICommandManager _commandManager;

        private readonly TabSpaceElementGenerator _elementGenerator;
        private readonly HighlightAllOccurencesOfSelectedWordTransformer _highlightAllOccurencesOfSelectedWordTransformer;
        private readonly object _scope;
        private readonly TextEditor _textEditor;
        private readonly IUIVisualizerService _uiVisualizerService;

        private bool _isInCustomUpdate = false;
        private bool _isInRedoUndo = false;

        private int _previousCaretColumn;
        private int _previousCaretLine;
        #endregion

        #region Constructors
        public CsvTextEditorService(object scope, TextEditor textEditor, ICommandManager commandManager, IUIVisualizerService uiVisualizerService)
        {
            Argument.IsNotNull(() => textEditor);
            Argument.IsNotNull(() => commandManager);
            Argument.IsNotNull(() => uiVisualizerService);

            _scope = scope;
            _textEditor = textEditor;
            _commandManager = commandManager;
            _uiVisualizerService = uiVisualizerService;

            // Need to make these options accessible to the user in the settings window
            _textEditor.ShowLineNumbers = true;
            _textEditor.Options.HighlightCurrentLine = true;
            _textEditor.Options.ShowEndOfLine = true;
            _textEditor.Options.ShowTabs = true;

            var serviceLocator = this.GetServiceLocator();
            var typeFactory = serviceLocator.ResolveType<ITypeFactory>();
            _elementGenerator = typeFactory.CreateInstance<TabSpaceElementGenerator>();

            _textEditor.TextArea.TextView.ElementGenerators.Add(_elementGenerator);

            _textEditor.TextArea.SelectionChanged += OnTextAreaSelectionChanged;
            _textEditor.TextArea.Caret.PositionChanged += OnCaretPositionChanged;
            _textEditor.TextChanged += OnTextChanged;

            _highlightAllOccurencesOfSelectedWordTransformer = new HighlightAllOccurencesOfSelectedWordTransformer();
            _textEditor.TextArea.TextView.LineTransformers.Add(_highlightAllOccurencesOfSelectedWordTransformer);

            _textEditor.TextArea.TextView.LineTransformers.Add(new FirstLineAlwaysBoldTransformer());
        }
        #endregion

        #region Properties
        public bool IsDirty { get; set; }
        public bool HasSelection => _textEditor.SelectionLength > 0;
        public bool CanRedo => _textEditor.CanRedo;
        public bool CanUndo => _textEditor.CanUndo;
        #endregion

        #region Methods
        public event EventHandler<CaretTextLocationChangedEventArgs> CaretTextLocationChanged;
        public event EventHandler<EventArgs> TextChanged;

        public void Copy()
        {
            _textEditor.Copy();
        }

        public void Paste()
        {
            var text = Clipboard.GetText();
            text = text.Replace(Symbols.Comma.ToString(), string.Empty)
                .Replace(_elementGenerator.NewLine, string.Empty);

            var offset = _textEditor.CaretOffset;
            _textEditor.Document.Insert(offset, text);
        }

        public void Redo()
        {
            using (new DisposableToken<CsvTextEditorService>(this, x => x.Instance._isInRedoUndo = true, x =>
            {
                RefreshView();
                x.Instance._isInRedoUndo = false;
            }))
            {
                _textEditor.Redo();
            }
        }

        public void Undo()
        {
            using (new DisposableToken<CsvTextEditorService>(this, x => x.Instance._isInRedoUndo = true, x =>
            {
                RefreshView();
                x.Instance._isInRedoUndo = false;
            }))
            {
                _textEditor.Undo();
            }
        }

        public void Cut()
        {
            var selectedText = _textEditor.SelectedText;

            ClearSelectedText();

            Clipboard.SetText(selectedText);
        }

        public void ShowFindReplaceDialog()
        {
            var serviceLocator = this.GetServiceLocator();
            var typeFactory = serviceLocator.ResolveType<ITypeFactory>();

            var findReplaceViewModel = typeFactory.CreateInstanceWithParametersAndAutoCompletion<FindReplaceDialogViewModel>(_scope);

            _uiVisualizerService.ShowAsync(findReplaceViewModel);
        }

        public void AddColumn()
        {
            var textDocument = _textEditor.Document;
            var linesCount = textDocument.LineCount;
            var offset = _textEditor.CaretOffset;

            var affectedLocation = textDocument.GetLocation(offset);
            var columnNumberWithOffset = _elementGenerator.GetColumn(affectedLocation);

            var columnsCount = _elementGenerator.ColumnCount;
            var newLine = _elementGenerator.NewLine;

            var columnLenght = columnNumberWithOffset.Length;
            var columnOffset = columnNumberWithOffset.OffsetInLine;

            var lineIndex = affectedLocation.Line - 1;
            var columnIndex = columnNumberWithOffset.ColumnNumber + 1;

            if (affectedLocation.Column == columnOffset)
            {
                var oldText = textDocument.Text;
                var newText = oldText.InsertCommaSeparatedColumn(columnIndex, linesCount, columnsCount, newLine);

                UpdateText(newText);
                Goto(lineIndex, columnIndex);

                return;
            }

            if (affectedLocation.Column == columnOffset - columnLenght + 1)
            {
                columnIndex--;

                var oldText = textDocument.Text;
                var newText = oldText.InsertCommaSeparatedColumn(columnIndex, linesCount, columnsCount, newLine);

                UpdateText(newText);
                Goto(lineIndex, columnIndex);
            }
        }

        public void RemoveColumn()
        {
            var textDocument = _textEditor.Document;
            var linesCount = textDocument.LineCount;
            var offset = _textEditor.CaretOffset;

            var affectedLocation = textDocument.GetLocation(offset);
            var columnNumberWithOffset = _elementGenerator.GetColumn(affectedLocation);

            var columnsCount = _elementGenerator.ColumnCount;
            var newLine = _elementGenerator.NewLine;

            var lineIndex = affectedLocation.Line - 1;
            var columnIndex = columnNumberWithOffset.ColumnNumber;

            var text = _textEditor.Text.RemoveCommaSeparatedColumn(columnIndex, linesCount, columnsCount, newLine);

            UpdateText(text);
            Goto(lineIndex, columnIndex);
        }

        public void AddLine()
        {
            var offset = _textEditor.CaretOffset;
            var textDocument = _textEditor.Document;
            var affectedLocation = textDocument.GetLocation(offset);

            var nextLineIndex = affectedLocation.Line;
            var affectedColumn = affectedLocation.Column;
            var insertOffsetInLine = affectedColumn - 1;

            if (affectedColumn == 1)
            {
                nextLineIndex--;
            }

            var columnNumberWithOffset = _elementGenerator.GetColumn(affectedLocation);

            var columnNumber = columnNumberWithOffset.ColumnNumber;
            var columnOffset = columnNumberWithOffset.OffsetInLine;

            var columnsCount = _elementGenerator.ColumnCount;
            var newLine = _elementGenerator.NewLine;

            var caretColumnIndex = columnNumber;
            if (columnNumber == columnsCount - 1 && affectedColumn == columnOffset)
            {
                caretColumnIndex = 0;
            }

            var oldText = _textEditor.Text;
            var text = oldText.InsertLineWithTextTransfer(nextLineIndex, insertOffsetInLine, columnsCount, newLine);

            UpdateText(text);
            Goto(nextLineIndex, caretColumnIndex);
        }

        public void DuplicateLine()
        {
            var textDocument = _textEditor.Document;
            var offset = _textEditor.CaretOffset;

            var affectedLocation = textDocument.GetLocation(offset);
            var columnNumberWithOffset = _elementGenerator.GetColumn(affectedLocation);
            var newLine = _elementGenerator.NewLine;

            var lineIndex = affectedLocation.Line - 1;
            var columnIndex = columnNumberWithOffset.ColumnNumber;

            var line = textDocument.Lines[lineIndex];
            var lineOffset = line.Offset;
            var endlineOffset = line.NextLine?.Offset ?? line.EndOffset;

            var text = _textEditor.Text.DuplicateTextInLine(lineOffset, endlineOffset, newLine);

            UpdateText(text);
            Goto(lineIndex + 1, columnIndex);
        }

        public void RemoveLine()
        {
            var textDocument = _textEditor.Document;
            var offset = _textEditor.CaretOffset;

            var affectedLocation = textDocument.GetLocation(offset);
            var columnNumberWithOffset = _elementGenerator.GetColumn(affectedLocation);
            var newLine = _elementGenerator.NewLine;

            var lineIndex = affectedLocation.Line - 1;
            var columnIndex = columnNumberWithOffset.ColumnNumber;

            var line = textDocument.Lines[lineIndex];
            var lineOffset = line.Offset;
            var endlineOffset = line.NextLine?.Offset ?? line.EndOffset;

            var text = _textEditor.Text.RemoveText(lineOffset, endlineOffset, newLine);

            UpdateText(text);

            Goto(lineIndex - 1, columnIndex);
        }

        public void DeleteNextSelectedText()
        {
            var selectionLenght = _textEditor.SelectionLength;
            if (selectionLenght == 0)
            {
                var deletePosition = _textEditor.SelectionStart;
                DeleteFromPosition(deletePosition);
                return;
            }

            ClearSelectedText();
        }

        public void DeletePreviousSelectedText()
        {
            var selectionLenght = _textEditor.SelectionLength;
            if (selectionLenght == 0)
            {
                var deletePosition = _textEditor.SelectionStart - 1;
                DeleteFromPosition(deletePosition);
                return;
            }

            ClearSelectedText();
        }

        public void Initialize(string text)
        {
            var document = _textEditor.Document;
            document.Changed -= OnTextDocumentChanged;

            UpdateText(text);

            _textEditor.Document.UndoStack.ClearAll();

            document.Changed += OnTextDocumentChanged;

            RefreshHighlightings();
        }

        public void GotoNextColumn()
        {
            var textDocument = _textEditor.Document;
            var offset = _textEditor.CaretOffset;

            var affectedLocation = textDocument.GetLocation(offset);
            var columnNumberWithOffset = _elementGenerator.GetColumn(affectedLocation);

            var columnsCount = _elementGenerator.ColumnCount;
            var nextColumnIndex = columnNumberWithOffset.ColumnNumber + 1;
            var lineIndex = affectedLocation.Line - 1;
            var nextLineIndex = lineIndex + 1;

            if (nextColumnIndex == columnsCount)
            {
                var linesCount = textDocument.LineCount;
                if (nextLineIndex == linesCount)
                {
                    return;
                }

                Goto(nextLineIndex, 0);
                return;
            }

            Goto(lineIndex, nextColumnIndex);
        }

        public void GotoPreviousColumn()
        {
            var textDocument = _textEditor.Document;
            var offset = _textEditor.CaretOffset;

            var affectedLocation = textDocument.GetLocation(offset);
            var columnNumberWithOffset = _elementGenerator.GetColumn(affectedLocation);

            var columnIndex = columnNumberWithOffset.ColumnNumber;
            var previousColumnIndex = columnIndex > 0 ? columnIndex - 1 : -1;

            var lineIndex = affectedLocation.Line - 1;
            var previousLineIndex = lineIndex > 0 ? lineIndex - 1 : -1;

            if (previousColumnIndex == -1)
            {
                if (previousLineIndex == -1)
                {
                    return;
                }

                var columnsCount = _elementGenerator.ColumnCount;
                Goto(previousLineIndex, columnsCount - 1);
                return;
            }

            Goto(lineIndex, previousColumnIndex);
        }

        public void RefreshView()
        {
            _elementGenerator.Refresh(_textEditor.Text);
            _textEditor.TextArea.TextView.Redraw();
        }
        #endregion

        private void RefreshHighlightings()
        {
            using (var s = GetType().Assembly.GetManifestResourceStream("Orc.CsvTextEditor.Resources.Highlightings.CustomHighlighting.xshd"))
            {
                if (s == null)
                {
                    throw new InvalidOperationException("Could not find embedded resource");
                }

                using (var reader = new XmlTextReader(s))
                {
                    _textEditor.SyntaxHighlighting = HighlightingLoader.Load(reader, HighlightingManager.Instance);
                }
            }
        }

        private void RefreshLocation(int offset, int length)
        {
            if (_isInCustomUpdate || _isInRedoUndo)
            {
                return;
            }

            var textDocument = _textEditor.Document;
            var affectedLocation = textDocument.GetLocation(offset);

            if (_elementGenerator.RefreshLocation(affectedLocation, length))
            {
                _textEditor.TextArea.TextView.Redraw();
            }
        }

        private void OnTextDocumentChanged(object sender, DocumentChangeEventArgs e)
        {
            RefreshLocation(e.Offset, e.InsertionLength - e.RemovalLength);
        }

        private void ClearSelectedText()
        {
            var textDocument = _textEditor.Document;

            var selectionStart = _textEditor.SelectionStart;
            var selectionLenght = _textEditor.SelectionLength;

            if (selectionLenght == 0)
            {
                return;
            }

            var newLine = _elementGenerator.NewLine;

            var text = textDocument.Text.RemoveCommaSeparatedText(selectionStart, selectionLenght, newLine);

            _textEditor.SelectionLength = 0;

            UpdateText(text);
            _textEditor.CaretOffset = selectionStart;
        }

        private void DeleteFromPosition(int deletePosition)
        {
            var textDocument = _textEditor.Document;

            if (deletePosition < 0 || deletePosition >= textDocument.TextLength)
            {
                return;
            }

            var deletingChar = textDocument.Text[deletePosition];
            if (deletingChar == Symbols.NewLineStart || deletingChar == Symbols.Comma || deletingChar == Symbols.NewLineEnd)
            {
                return;
            }

            textDocument.Remove(deletePosition, 1);
        }

        private void UpdateText(string text)
        {
            text = text ?? string.Empty;

            _elementGenerator.Refresh(text);

            _isInCustomUpdate = true;

            using (_textEditor.Document.RunUpdate())
            {
                _textEditor.Document.Text = text;
            }

            _isInCustomUpdate = false;
        }

        private void OnTextChanged(object sender, EventArgs eventArgs)
        {
            TextChanged?.Invoke(this, EventArgs.Empty);
        }

        private void OnCaretPositionChanged(object sender, EventArgs eventArgs)
        {
            var offset = _textEditor.CaretOffset;
            var textDocument = _textEditor.Document;
            var currentTextLocation = textDocument.GetLocation(offset);
            var columnNumberWithOffset = _elementGenerator.GetColumn(currentTextLocation);
            var column = columnNumberWithOffset.ColumnNumber + 1;
            var line = currentTextLocation.Line + 1;

            if (_previousCaretColumn != column || _previousCaretLine != line)
            {
                CaretTextLocationChanged?.Invoke(this, new CaretTextLocationChangedEventArgs(column, line));

                _previousCaretColumn = column;
                _previousCaretLine = line;
            }
        }

        private void Goto(int lineIndex, int columnIndex)
        {
            _textEditor.SetCaretToSpecificLineAndColumn(lineIndex, columnIndex, _elementGenerator.Lines);
        }

        private void OnTextAreaSelectionChanged(object sender, EventArgs e)
        {
            _commandManager.InvalidateCommands();

            // Disable this line if the user is using the "Find Replace" dialog box
            _highlightAllOccurencesOfSelectedWordTransformer.SelectedWord = _textEditor.SelectedText;
            _highlightAllOccurencesOfSelectedWordTransformer.Selection = _textEditor.TextArea.Selection;

            RefreshView();
        }
    }
}