// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CsvTextEditorToolBase.cs" company="WildGums">
//   Copyright (c) 2008 - 2016 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.CsvTextEditor.CsvTextEditorToolManagement
{
    using System;
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
        public string Name => "CsvTextEditor.FindReplaceTool";
        #endregion

        #region Methods
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

        public event EventHandler<EventArgs> Closed;
        #endregion

        protected void RaiseClosedEvent()
        {
            Closed?.Invoke(this, EventArgs.Empty);
        }

        public abstract void OnInitialize(object scope = null);

        protected bool Equals(CsvTextEditorToolBase other)
        {
            return Equals(Name, other.Name);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
            {
                return false;
            }
            if (ReferenceEquals(this, obj))
            {
                return true;
            }
            if (obj.GetType() != this.GetType())
            {
                return false;
            }
            return Equals((CsvTextEditorToolBase) obj);
        }

        public override int GetHashCode()
        {
            return (TexEditor != null ? TexEditor.GetHashCode() : 0);
        }
    }
}