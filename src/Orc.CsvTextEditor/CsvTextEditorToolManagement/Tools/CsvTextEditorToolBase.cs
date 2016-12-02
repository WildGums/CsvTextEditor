// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CsvTextEditorToolBase.cs" company="WildGums">
//   Copyright (c) 2008 - 2016 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.CsvTextEditor.CsvTextEditorToolManagement
{
    using System.Collections.Generic;
    using ICSharpCode.AvalonEdit;

    public abstract class CsvTextEditorToolBase : ICsvTextEditorTool
    {
        #region Fields
        private readonly HashSet<object> _initializedInScopes;
        #endregion

        #region Constructors
        public CsvTextEditorToolBase()
        {
            _initializedInScopes = new HashSet<object>();
        }
        #endregion

        #region Properties
        protected TextEditor TexEditor { get; private set; }
        #endregion

        #region Methods
        public string Name => "CsvTextEditor.FindReplaceTool";

        public bool IsInitialized(object scope = null)
        {
            return _initializedInScopes.Contains(scope);
        }

        public abstract void Open();
        public abstract void Close();

        public void Initialize(TextEditor textEditor, object scope)
        {
            TexEditor = textEditor;

            OnInitialize(scope);

            _initializedInScopes.Add(scope);
        }
        #endregion

        public abstract void OnInitialize(object scope = null);
    }
}