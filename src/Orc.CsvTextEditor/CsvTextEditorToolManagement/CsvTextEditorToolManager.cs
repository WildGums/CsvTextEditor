// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CsvTextEditorToolManager.cs" company="WildGums">
//   Copyright (c) 2008 - 2016 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.CsvTextEditor.CsvTextEditorToolManagement
{
    using System.Collections.Generic;
    using System.Linq;
    using Catel;
    using Catel.IoC;

    public class CsvTextEditorToolManager : ICsvTextEditorToolManager
    {
        #region Fields
        private readonly CsvTextEditorControl _textEditorControl;
        private readonly List<ICsvTextEditorTool> _tools;
        private readonly ITypeFactory _typeFactory;
        #endregion

        #region Constructors
        public CsvTextEditorToolManager(CsvTextEditorControl textEditorControl, ITypeFactory typeFactory)
        {
            Argument.IsNotNull(() => textEditorControl);
            Argument.IsNotNull(() => typeFactory);

            _textEditorControl = textEditorControl;
            _typeFactory = typeFactory;

            _tools = new List<ICsvTextEditorTool>();
        }
        #endregion

        #region Properties
        public IEnumerable<ICsvTextEditorTool> Tools => _tools.AsEnumerable();
        public object Scope => _textEditorControl?.Scope;
        #endregion

        #region Methods
        public void AddTool<T>()
            where T : ICsvTextEditorTool
        {
            var tool = _typeFactory.CreateInstanceWithParametersAndAutoCompletion<T>();

            AddTool(tool);
        }

        public void AddTool(ICsvTextEditorTool tool)
        {
            Argument.IsNotNull(() => tool);

            if (_tools.Contains(tool))
            {
                return;
            }

            var scope = Scope;
            if (!tool.IsInitialized(scope))
            {
                var textEditor = _textEditorControl?.TextEditor;

                tool.Initialize(textEditor, scope);
            }

            tool.Open();
        }

        public void RemoveToolByName(string toolName)
        {
            var tool = _tools.FirstOrDefault(x => x.Name == toolName);
            if (tool == null)
            {
                return;
            }

            _tools.Remove(tool);
            tool.Close();
        }

        public void RemoveTool<T>()
            where T : ICsvTextEditorTool
        {
            var tool = _tools.FirstOrDefault(x => x.GetType() == typeof (T));
            if (tool == null)
            {
                return;
            }

            _tools.Remove(tool);
            tool.Close();
        }
        #endregion
    }
}