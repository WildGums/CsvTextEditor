// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ICsvTextEditorService.cs" company="WildGums">
//   Copyright (c) 2008 - 2016 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.CsvTextEditor.Services
{
    using System;

    public interface ICsvTextEditorService
    {
        #region Properties
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

        void RefreshLocation(int offset, int length);

        event EventHandler<CaretTextLocationChangedEventArgs> CaretTextLocationChanged;
        event EventHandler<EventArgs> TextChanged;
    }
}