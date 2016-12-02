// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ICsvTextEditorFindReplaceSerivce.cs" company="WildGums">
//   Copyright (c) 2008 - 2016 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.CsvTextEditor.Services
{
    using Controls;

    public interface ICsvTextEditorFindReplaceSerivce
    {
        bool FindNext(string textToFind, FindReplaceSettings settings);
        bool Replace(string textToFind, string textToReplace, FindReplaceSettings settings);
        void ReplaceAll(string textToFind, string textToReplace, FindReplaceSettings settings);
    }
}