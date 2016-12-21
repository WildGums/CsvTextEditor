// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ICsvTextEditorTool.cs" company="WildGums">
//   Copyright (c) 2008 - 2016 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.CsvTextEditor
{
    using System;

    public interface ICsvTextEditorTool
    {
        #region Properties
        string Name { get; }
        bool IsOpened { get; }
        #endregion

        void Open();
        void Close();

        event EventHandler<EventArgs> Opened;
        event EventHandler<EventArgs> Closed;
    }
}