// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IPostprocessor.cs" company="WildGums">
//   Copyright (c) 2008 - 2016 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.CsvTextEditor
{
    public interface IPostprocessor
    {
        void Apply();
        void RestoreCaret();
    }
}