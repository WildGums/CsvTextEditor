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
        #endregion

        #region Constructors
        public AddColumnPostprocessor(DocumentChangingContext documentChangingContext)
        {
            Argument.IsNotNull(() => documentChangingContext);

            _documentChangingContext = documentChangingContext;

            _textEditor = _documentChangingContext.TextEditor;
            _textDocument = _textEditor.Document;
            _elementGenerator = _documentChangingContext.ElementGenerator;

            _columnsCount = _elementGenerator.Lines[0].Length;
            _linesCount = _textEditor.Document.LineCount;

            var offset = _documentChangingContext.Offset;
            var widthCalculator = _documentChangingContext.ColumnWidthCalculator;
            var affectedLocation = _textDocument.GetLocation(offset);
            var columnWidthByLine = _elementGenerator.Lines;
            var columnNumberWithOffset = widthCalculator.GetColumn(columnWidthByLine, affectedLocation);

            _columnIndex = columnNumberWithOffset.ColumnNumber;
            _lineIndex = affectedLocation.Line - 1;
        }
        #endregion

        #region Methods
        public void Apply()
        {
            var oldText = _documentChangingContext.OldText;
            _textEditor.Text = oldText.InsertCommaSeparatedColumn(_columnIndex, _linesCount, _columnsCount);
        }

        public void RestoreCaret()
        {
            SetCaretToSpecificLineAndColumn(_lineIndex, _columnIndex);
        }

        private void SetCaretToSpecificLineAndColumn(int lineIndex, int columnIndex)
        {
            var line = _textDocument.Lines[lineIndex];
            var offset = line.Offset;
            var columnOffset = _elementGenerator.Lines[lineIndex].Take(columnIndex + 1).Sum();

            _textEditor.CaretOffset = offset + columnOffset;
        }
        #endregion
    }
}