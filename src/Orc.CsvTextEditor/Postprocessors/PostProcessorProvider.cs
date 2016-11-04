// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PostprocessorProvider.cs" company="WildGums">
//   Copyright (c) 2008 - 2016 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.CsvTextEditor
{
    using System.Collections.Generic;

    internal class PostprocessorProvider : IPostprocessorProvider
    {
        public IPostprocessor GetPostprocessors(string text, DocumentChangingContext documentChangingContext)
        {
            var insertedText = documentChangingContext.InsertedText;
            if (insertedText.Length != 1 || insertedText[0] != Symbols.Comma)
            {
                return null;
            }            

            return new AddColumnPostprocessor(documentChangingContext);
        }
    }
}