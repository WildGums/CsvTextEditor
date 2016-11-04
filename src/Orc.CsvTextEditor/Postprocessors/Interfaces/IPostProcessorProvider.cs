// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IPostprocessorProvider.cs" company="WildGums">
//   Copyright (c) 2008 - 2016 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.CsvTextEditor
{
    using System.Collections.Generic;

    internal interface IPostprocessorProvider
    {
        IPostprocessor GetPostprocessors(string text, DocumentChangingContext documentChangingContext);
    }
}