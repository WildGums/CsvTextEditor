// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CsvTextEditorToolBase.cs" company="WildGums">
//   Copyright (c) 2008 - 2017 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.CsvTextEditor
{
    using System;
    using Catel;
    using ICSharpCode.AvalonEdit;
    using Services;

    public abstract class CsvTextEditorToolBase : ICsvTextEditorTool
    {
        #region Constructors
        public CsvTextEditorToolBase(TextEditor textEditor, ICsvTextEditorService csvTextEditorService)
        {
            Argument.IsNotNull(() => textEditor);
            Argument.IsNotNull(() => csvTextEditorService);

            TextEditor = textEditor;
            CsvTextEditorService = csvTextEditorService;
        }
        #endregion

        #region Properties
        protected TextEditor TextEditor { get; }
        protected ICsvTextEditorService CsvTextEditorService { get; private set; }
        #endregion

        #region Methods
        public abstract string Name { get; }
        public bool IsOpened { get; private set; }

        public void Open()
        {
            if (IsOpened)
            {
                return;
            }

            OnOpen();

            IsOpened = true;
            Opened?.Invoke(this, EventArgs.Empty);
        }
        public abstract void Close();
        protected abstract void OnOpen();

        public event EventHandler<EventArgs> Closed;
        public event EventHandler<EventArgs> Opened;
        #endregion

        protected void RaiseClosedEvent()
        {
            Closed?.Invoke(this, EventArgs.Empty);
        }
    }
}