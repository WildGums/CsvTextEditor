// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AddColumnPostprocessor.cs" company="WildGums">
//   Copyright (c) 2008 - 2016 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.CsvTextEditor
{
    using Catel;
    using ICSharpCode.AvalonEdit;

    internal class AddColumnPostprocessor : IPostprocessor
    {
        #region Fields
        private readonly int _columnIndex;
        private readonly int _columnsCount;
        private readonly DocumentChangingContext _documentChangingContext;
        private readonly TabSpaceElementGenerator _elementGenerator;
        private readonly int _lineIndex;
        private readonly int _linesCount;
        private readonly int _offset;
        private readonly bool _restoreText;
        private readonly TextEditor _textEditor;
        #endregion

        #region Constructors
        public AddColumnPostprocessor(DocumentChangingContext documentChangingContext)
        {
            Argument.IsNotNull(() => documentChangingContext);

            _restoreText = false;

            _documentChangingContext = documentChangingContext;

            _textEditor = _documentChangingContext.TextEditor;
            var textDocument = _textEditor.Document;
            _elementGenerator = _documentChangingContext.ElementGenerator;

            _columnsCount = _elementGenerator.Lines[0].Length;
            _linesCount = _textEditor.Document.LineCount;

            _offset = _documentChangingContext.Offset;
            var widthCalculator = _documentChangingContext.ColumnWidthCalculator;
            var affectedLocation = textDocument.GetLocation(_offset);
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

            _textEditor.SetCaretToSpecificLineAndColumn(_lineIndex, _columnIndex, _elementGenerator.Lines);
        }
        #endregion
    }
}