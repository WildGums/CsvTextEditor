// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ICsvTextEditorTool.cs" company="WildGums">
//   Copyright (c) 2008 - 2016 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.CsvTextEditor
{
    using System;
    using ICSharpCode.AvalonEdit;
    using Services;

    public interface ICsvTextEditorTool
    {
        #region Properties
        string Name { get; }
        bool IsInitialized { get; }
        #endregion

        void Open();
        void Close();
        void Initialize(TextEditor textEditor, ICsvTextEditorService csvTextEditorService);

        event EventHandler<EventArgs> Closed;
    }
}