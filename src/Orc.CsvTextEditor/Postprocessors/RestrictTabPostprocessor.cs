// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RestrictTabPostprocessor.cs" company="WildGums">
//   Copyright (c) 2008 - 2016 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.CsvTextEditor
{
    using Catel;
    using ICSharpCode.AvalonEdit;

    internal class RestrictTabPostprocessor : IPostprocessor
    {
        #region Fields
        private readonly DocumentChangingContext _documentChangingContext;
        private readonly int _offset;
        private readonly TextEditor _textEditor;
        #endregion

        #region Constructors
        public RestrictTabPostprocessor(DocumentChangingContext documentChangingContext)
        {
            Argument.IsNotNull(() => documentChangingContext);

            _documentChangingContext = documentChangingContext;
            _offset = _documentChangingContext.Offset;
            _textEditor = documentChangingContext.TextEditor;
        }
        #endregion

        #region Methods
        public void Apply()
        {
            var oldText = _documentChangingContext.OldText;
            _textEditor.Text = oldText;
        }

        public void RestoreCaret()
        {
            _textEditor.CaretOffset = _offset;
        }
        #endregion
    }
}