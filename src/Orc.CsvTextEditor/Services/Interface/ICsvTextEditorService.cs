// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ICsvTextEditorService.cs" company="WildGums">
//   Copyright (c) 2008 - 2016 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.CsvTextEditor.Services
{
    using System;
    using System.Collections.Generic;

    public interface ICsvTextEditorService
    {
        #region Properties
        IEnumerable<ICsvTextEditorTool> Tools { get; } 

        bool IsDirty { get; set; }
        bool HasSelection { get; }
        bool CanRedo { get; }
        bool CanUndo { get; }
        #endregion

        void Copy();
        void Cut();
        void Paste();
        void Redo();
        void Undo();

        void Initialize(string text);

        void AddColumn();
        void AddLine();
        void RemoveColumn();
        void DuplicateLine();
        void RemoveLine();
        void DeleteNextSelectedText();
        void DeletePreviousSelectedText();

        void GotoNextColumn();
        void GotoPreviousColumn();

        void RefreshView();

        void AddTool(ICsvTextEditorTool tool);
        void RemoveTool(ICsvTextEditorTool tool);

        event EventHandler<CaretTextLocationChangedEventArgs> CaretTextLocationChanged;
        event EventHandler<EventArgs> TextChanged;
    }
}