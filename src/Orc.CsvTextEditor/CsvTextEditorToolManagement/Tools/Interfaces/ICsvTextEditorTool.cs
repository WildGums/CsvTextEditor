// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ICsvTextEditorTool.cs" company="WildGums">
//   Copyright (c) 2008 - 2016 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.CsvTextEditor.CsvTextEditorToolManagement
{
    using ICSharpCode.AvalonEdit;

    public interface ICsvTextEditorTool
    {
        #region Properties
        string Name { get; }
        #endregion

        bool IsInitialized(object scope = null);
        void Open();
        void Close();
        void Initialize(TextEditor textEditor, object scope = null);
    }
}