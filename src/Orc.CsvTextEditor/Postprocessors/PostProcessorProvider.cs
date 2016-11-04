// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PostprocessorProvider.cs" company="WildGums">
//   Copyright (c) 2008 - 2016 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.CsvTextEditor
{
    using System;

    internal class PostprocessorProvider : IPostprocessorProvider
    {
        #region Methods
        public IPostprocessor GetPostprocessors(string text, DocumentChangingContext documentChangingContext)
        {
            var insertedText = documentChangingContext.InsertedText;
            if (string.Equals(insertedText, Symbols.Comma.ToString()))
            {
                return new AddColumnPostprocessor(documentChangingContext);
            }

            if (string.Equals(insertedText, Environment.NewLine))
            {
                return new AddLineWithTextTransferPostprocessor(documentChangingContext);
                //Note: add this processor if you want to add new line without text transfer to another line
                //new AddLinePostprocessor(documentChangingContext);
            }

            if (string.Equals(insertedText, Symbols.HorizontalTab.ToString()))
            {
                return new RestrictTabPostprocessor(documentChangingContext);
            }

            return null;
        }
        #endregion
    }
}