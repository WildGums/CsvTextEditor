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
                return new AddLinePostprocessor(documentChangingContext);
            }

            return null;
        }
        #endregion
    }
}