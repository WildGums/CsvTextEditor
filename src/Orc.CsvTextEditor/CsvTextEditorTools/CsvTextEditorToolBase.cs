// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CsvTextEditorToolBase.cs" company="WildGums">
//   Copyright (c) 2008 - 2016 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.CsvTextEditor
{
    using System;
    using ICSharpCode.AvalonEdit;
    using Services;

    public abstract class CsvTextEditorToolBase : ICsvTextEditorTool
    {
        #region Fields
        public bool IsInitialized { get; private set; } = false;
        #endregion

        #region Properties
        protected TextEditor TexEditor { get; private set; }
        public string Name => "CsvTextEditor.FindReplaceTool";
        #endregion

        #region Methods

        public abstract void Open();
        public abstract void Close();

        public void Initialize(TextEditor textEditor, ICsvTextEditorService csvTextEditorService)
        {
            TexEditor = textEditor;

            OnInitialize(csvTextEditorService);

            IsInitialized = true;
        }

        public event EventHandler<EventArgs> Closed;
        #endregion

        protected void RaiseClosedEvent()
        {
            Closed?.Invoke(this, EventArgs.Empty);
        }

        public abstract void OnInitialize(ICsvTextEditorService csvTextEditorService);

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
            return TexEditor?.GetHashCode() ?? 0;
        }
    }
}