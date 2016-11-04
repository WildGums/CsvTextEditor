// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AddColumnPostprocessor.cs" company="WildGums">
//   Copyright (c) 2008 - 2016 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.CsvTextEditor
{
    using System.Linq;
    using Catel;
    using ICSharpCode.AvalonEdit;
    using ICSharpCode.AvalonEdit.Document;

    internal class AddColumnPostprocessor : IPostprocessor
    {
        #region Fields
        private readonly DocumentChangingContext _documentChangingContext;
        private readonly TabSpaceElementGenerator _elementGenerator;
        private readonly TextDocument _textDocument;
        private readonly TextEditor _textEditor;
        private readonly int _columnsCount;
        private readonly int _linesCount;
        private readonly int _columnIndex;
        private readonly int _lineIndex;
        private readonly int _offset;
        private readonly bool _restoreText;
        #endregion

        #region Constructors
        public AddColumnPostprocessor(DocumentChangingContext documentChangingContext)
        {
            Argument.IsNotNull(() => documentChangingContext);

            _restoreText = false;

            _documentChangingContext = documentChangingContext;

            _textEditor = _documentChangingContext.TextEditor;
            _textDocument = _textEditor.Document;
            _elementGenerator = _documentChangingContext.ElementGenerator;

            _columnsCount = _elementGenerator.Lines[0].Length;
            _linesCount = _textEditor.Document.LineCount;

            _offset = _documentChangingContext.Offset;
            var widthCalculator = _documentChangingContext.ColumnWidthCalculator;
            var affectedLocation = _textDocument.GetLocation(_offset);
            var columnWidthByLine = _elementGenerator.Lines;
            var columnNumberWithOffset = widthCalculator.GetColumn(columnWidthByLine, affectedLocation);

            var columnLenght = columnNumberWithOffset.Length;
            var columnOffset = columnNumberWithOffset.OffsetInLine;

            _lineIndex = affectedLocation.Line - 1;
            _columnIndex = columnNumberWithOffset.ColumnNumber + 1;

            if (affectedLocation.Column == columnOffset)
            {
                return;
            }

            if (affectedLocation.Column == columnOffset - columnLenght + 1)
            {
                _columnIndex--;
                return;
            }

            _restoreText = true;
        }
        #endregion

        #region Methods
        public void Apply()
        {
            var oldText = _documentChangingContext.OldText;
            if (_restoreText)
            {
                _textEditor.Text = oldText;
                return;
            }

            _textEditor.Text = oldText.InsertCommaSeparatedColumn(_columnIndex, _linesCount, _columnsCount);
        }

        public void RestoreCaret()
        {
            if (_restoreText)
            {
                _textEditor.CaretOffset = _offset;
                return;
            }

            SetCaretToSpecificLineAndColumn(_lineIndex, _columnIndex);
        }

        private void SetCaretToSpecificLineAndColumn(int lineIndex, int columnIndex)
        {
            var line = _textDocument.Lines[lineIndex];
            var offset = line.Offset;
            var columnOffset = _elementGenerator.Lines[lineIndex].Take(columnIndex).Sum();

            _textEditor.CaretOffset = offset + columnOffset;
        }
        #endregion
    }
}