// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AddLineWithTextTransferPostprocessor.cs" company="WildGums">
//   Copyright (c) 2008 - 2016 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.CsvTextEditor
{
    using Catel;
    using ICSharpCode.AvalonEdit;

    internal class AddLineWithTextTransferPostprocessor : IPostprocessor
    {
        #region Fields
        private readonly int _columnsCount;
        private readonly DocumentChangingContext _documentChangingContext;
        private readonly TabSpaceElementGenerator _elementGenerator;
        private readonly int _nextLineIndex;
        private readonly TextEditor _textEditor;
        private readonly int _caretColumnIndex;
        private readonly int _insertOffsetInLine;
        #endregion

        #region Constructors
        public AddLineWithTextTransferPostprocessor(DocumentChangingContext documentChangingContext)
        {
            Argument.IsNotNull(() => documentChangingContext);

            _documentChangingContext = documentChangingContext;
            _textEditor = _documentChangingContext.TextEditor;

            _elementGenerator = _documentChangingContext.ElementGenerator;
            _columnsCount = _elementGenerator.Lines[0].Length;

            var offset = _documentChangingContext.Offset;
            var textDocument = _textEditor.Document;
            var affectedLocation = textDocument.GetLocation(offset);

            _nextLineIndex = affectedLocation.Line;
            _insertOffsetInLine = affectedLocation.Column - 1;

            if (affectedLocation.Column == 1)
            {
                _nextLineIndex--;
            }

            var columnWidthByLine = _elementGenerator.Lines;
            var widthCalculator = _documentChangingContext.ColumnWidthCalculator;
            var columnNumberWithOffset = widthCalculator.GetColumn(columnWidthByLine, affectedLocation);

            var columnNumber = columnNumberWithOffset.ColumnNumber;
            var columnOffset = columnNumberWithOffset.OffsetInLine;
            if (columnNumber == _columnsCount - 1 && affectedLocation.Column == columnOffset)
            {
                _caretColumnIndex = 0;
            }
            else
            {
                _caretColumnIndex = columnNumber;
            }
        }
        #endregion

        #region Methods
        public void Apply()
        {
            var oldText = _documentChangingContext.OldText;
            _textEditor.Text = oldText.InsertLineWithTextTransfer(_nextLineIndex, _insertOffsetInLine, _columnsCount);
        }

        public void RestoreCaret()
        {
            _textEditor.SetCaretToSpecificLineAndColumn(_nextLineIndex, _caretColumnIndex, _elementGenerator.Lines);
        }
        #endregion
    }
}