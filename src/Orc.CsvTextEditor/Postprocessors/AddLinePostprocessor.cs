// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AddLinePostprocessor.cs" company="WildGums">
//   Copyright (c) 2008 - 2016 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.CsvTextEditor
{
    using Catel;
    using ICSharpCode.AvalonEdit;

    internal class AddLinePostprocessor : IPostprocessor
    {
        #region Fields
        private readonly int _columnsCount;
        private readonly DocumentChangingContext _documentChangingContext;
        private readonly TabSpaceElementGenerator _elementGenerator;
        private readonly int _nextLineIndex;
        private readonly TextEditor _textEditor;
        #endregion

        #region Constructors
        public AddLinePostprocessor(DocumentChangingContext documentChangingContext)
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

            if (affectedLocation.Column == 1)
            {
                _nextLineIndex--;
            }
        }
        #endregion

        #region Methods
        public void Apply()
        {
            var oldText = _documentChangingContext.OldText;
            _textEditor.Text = oldText.InsertLine(_nextLineIndex, _columnsCount);
        }

        public void RestoreCaret()
        {
            _textEditor.SetCaretToSpecificLineAndColumn(_nextLineIndex, 0, _elementGenerator.Lines);
        }
        #endregion
    }
}