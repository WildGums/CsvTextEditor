// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ICsvTextEditorToolManager.cs" company="WildGums">
//   Copyright (c) 2008 - 2016 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.CsvTextEditor.CsvTextEditorToolManagement
{
    using System.Collections.Generic;

    public interface ICsvTextEditorToolManager
    {
        #region Properties
        object Scope { get; }
        IEnumerable<ICsvTextEditorTool> Tools { get; }
        #endregion

        void AddTool(ICsvTextEditorTool tool);
        void AddTool<T>() where T : ICsvTextEditorTool;
        void RemoveTool<T>() where T : ICsvTextEditorTool;
        void RemoveToolByName(string toolName);
    }
}