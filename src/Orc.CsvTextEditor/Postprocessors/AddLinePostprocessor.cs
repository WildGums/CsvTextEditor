// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AddLinePostprocessor.cs" company="WildGums">
//   Copyright (c) 2008 - 2016 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.CsvTextEditor
{
    using Catel;
    using ICSharpCode.AvalonEdit;
    using ICSharpCode.AvalonEdit.Document;

    internal class AddLinePostprocessor : IPostprocessor
    {
        #region Fields
        private readonly int _columnsCount;
        private readonly DocumentChangingContext _documentChangingContext;
        private readonly TabSpaceElementGenerator _elementGenerator;
        private readonly int _nextLineIndex;
        private readonly int _offset;
        private readonly TextDocument _textDocument;
        private readonly TextEditor _textEditor;
        #endregion

        #region Constructors
        public AddLinePostprocessor(DocumentChangingContext documentChangingContext)
        {
            Argument.IsNotNull(() => documentChangingContext);

            _documentChangingContext = documentChangingContext;

            _textEditor = _documentChangingContext.TextEditor;
            _textDocument = _textEditor.Document;
            _elementGenerator = _documentChangingContext.ElementGenerator;

            _columnsCount = _elementGenerator.Lines[0].Length;

            _offset = _documentChangingContext.Offset;
            var affectedLocation = _textDocument.GetLocation(_offset);

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