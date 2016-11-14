// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CsvTextEditorService.cs" company="WildGums">
//   Copyright (c) 2008 - 2016 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.CsvTextEditor.Services
{
    using System;
    using System.Linq;
    using System.Windows.Input;
    using Catel;
    using Catel.MVVM;
    using ICSharpCode.AvalonEdit;
    using ICSharpCode.AvalonEdit.Document;

    internal class CsvTextEditorService : ICsvTextEditorService
    {
        #region Fields
        private readonly ICommandManager _commandManager;

        private readonly TabSpaceElementGenerator _elementGenerator;
        private readonly TextEditor _textEditor;

        CommandBinding _redoBinding;
        CommandBinding _undoBinding;
        #endregion

        #region Constructors
        public CsvTextEditorService(TextEditor textEditor, TabSpaceElementGenerator tabSpaceElementGenerator, ICommandManager commandManager)
        {
            Argument.IsNotNull(() => textEditor);
            Argument.IsNotNull(() => tabSpaceElementGenerator);
            Argument.IsNotNull(() => commandManager);

            _textEditor = textEditor;
            _commandManager = commandManager;

            _elementGenerator = tabSpaceElementGenerator;
            _textEditor.TextArea.TextView.ElementGenerators.Add(_elementGenerator);

            _textEditor.TextArea.SelectionChanged += OnTextAreaSelectionChanged;
            _textEditor.TextChanged += OnTextChanged;

            AvalonEditCommands.DeleteLine.InputGestures.Add(new KeyGesture(Key.L, ModifierKeys.Control));
        }
        #endregion

        #region Properties
        public bool IsDirty { get; set; }
        public bool HasSelection => _textEditor.SelectionLength > 0;
        public bool CanRedo => _textEditor.CanRedo;
        public bool CanUndo => _textEditor.CanUndo;
        #endregion

        #region Methods
        public void Copy()
        {
            _textEditor.Copy();
        }

        public void Cut()
        {
            _textEditor.Cut();
        }

        public void Paste()
        {
            _textEditor.Paste();
        }

        public void Redo()
        {
            _textEditor.Redo();
        }

        public void Undo()
        {
            _textEditor.Undo();
        }

        public string AddColumnToDocument(int offset)
        {
            var textDocument = _textEditor.Document;

            var linesCount = textDocument.LineCount;

            var affectedLocation = textDocument.GetLocation(offset);
            var columnNumberWithOffset = _elementGenerator.GetColumn(affectedLocation);
            var columnCount = _elementGenerator.ColumnCount;

            var columnLenght = columnNumberWithOffset.Length;
            var columnOffset = columnNumberWithOffset.OffsetInLine;

            var lineIndex = affectedLocation.Line - 1;
            var columnIndex = columnNumberWithOffset.ColumnNumber + 1;

            var oldText = textDocument.Text;

            if (affectedLocation.Column == columnOffset)
            {
                return oldText.InsertCommaSeparatedColumn(columnIndex, linesCount, columnCount);
            }

            if (affectedLocation.Column == columnOffset - columnLenght + 1)
            {
                columnIndex--;

                return oldText.InsertCommaSeparatedColumn(columnIndex, linesCount, columnCount);
            }

            return oldText;
        }

        public void AddColumn()
        {
            var textDocument = _textEditor.Document;

            
            var linesCount = textDocument.LineCount;
            var offset = _textEditor.CaretOffset;

            var affectedLocation = textDocument.GetLocation(offset);

            var columnNumberWithOffset = _elementGenerator.GetColumn(affectedLocation);
            var columnsCount = _elementGenerator.ColumnCount;

            var columnLenght = columnNumberWithOffset.Length;
            var columnOffset = columnNumberWithOffset.OffsetInLine;

            var lineIndex = affectedLocation.Line - 1;
            var columnIndex = columnNumberWithOffset.ColumnNumber + 1;

            if (affectedLocation.Column == columnOffset)
            {
                var oldText = textDocument.Text;
                var newText = oldText.InsertCommaSeparatedColumn(columnIndex, linesCount, columnsCount);

                UpdateTextEditor(newText);
                _textEditor.SetCaretToSpecificLineAndColumn(lineIndex, columnIndex, _elementGenerator.Lines);
            }

            if (affectedLocation.Column == columnOffset - columnLenght + 1)
            {
                columnIndex--;

                var oldText = textDocument.Text;
                var newText = oldText.InsertCommaSeparatedColumn(columnIndex, linesCount, columnsCount);

                UpdateTextEditor(newText);
                _textEditor.SetCaretToSpecificLineAndColumn(lineIndex, columnIndex, _elementGenerator.Lines);
            }
        }

        public void UpdateTextEditor(string text)
        {
            var lines = text.Split(new[] {Environment.NewLine}, StringSplitOptions.None);

            var columnWidthByLine = lines.Select(x => x.Split(Symbols.Comma))
                .Select(x => x.Select(y => y.Length + 1).ToArray())
                .ToArray();

            _elementGenerator.Lines = columnWidthByLine;

            _textEditor.Text = text;
        }
        #endregion

        public Tuple<int, int> AddColumn(TextDocument document, int offset)
        {
            var textDocument = _textEditor.Document;
            
            var linesCount = textDocument.LineCount;

            var affectedLocation = textDocument.GetLocation(offset);

            var columnNumberWithOffset = _elementGenerator.GetColumn(affectedLocation);
            var columnsCount = _elementGenerator.ColumnCount;

            var columnLenght = columnNumberWithOffset.Length;
            var columnOffset = columnNumberWithOffset.OffsetInLine;

            var lineIndex = affectedLocation.Line - 1;
            var columnIndex = columnNumberWithOffset.ColumnNumber + 1;

            var oldText = textDocument.Text;

            if (affectedLocation.Column == columnOffset)
            {
                document.Text = oldText.InsertCommaSeparatedColumn(columnIndex, linesCount, columnsCount);
            }

            if (affectedLocation.Column == columnOffset - columnLenght + 1)
            {
                columnIndex--;

                document.Text = oldText.InsertCommaSeparatedColumn(columnIndex, linesCount, columnsCount);
            }

            return new Tuple<int, int>(lineIndex, columnIndex);
        }

        private void OnTextAreaSelectionChanged(object sender, EventArgs e)
        {
            _commandManager.InvalidateCommands();
        }

        private void OnTextChanged(object sender, EventArgs e)
        {
            _commandManager.InvalidateCommands();

            if (_undoBinding == null)
            {
                _undoBinding = new CommandBinding(
                    ApplicationCommands.Undo, new ExecutedRoutedEventHandler(UndoExecuted), null);
                _redoBinding = new CommandBinding(
                    ApplicationCommands.Redo, new ExecutedRoutedEventHandler(RedoExecuted), null);

                _textEditor.CommandBindings.Add(_undoBinding);
                _textEditor.CommandBindings.Add(_redoBinding);
            }
        }

        private void UndoExecuted(object sender, ExecutedRoutedEventArgs args)
        {
            //  ApplicationCommands.Undo.Execute(null, Application.Current.MainWindow);
        }

        private void RedoExecuted(object sender, ExecutedRoutedEventArgs args)
        {
            //    ApplicationCommands.Redo.Execute(null, Application.Current.MainWindow);
        }
    }
}