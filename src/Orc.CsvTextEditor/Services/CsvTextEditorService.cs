// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CsvTextEditorService.cs" company="WildGums">
//   Copyright (c) 2008 - 2016 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.CsvTextEditor.Services
{
    using System;
    using System.Windows;
    using System.Windows.Input;
    using Catel;
    using Catel.MVVM;
    using ICSharpCode.AvalonEdit;

    internal class CsvTextEditorService : ICsvTextEditorService
    {
        #region Fields
        private readonly ICommandManager _commandManager;
        private readonly TextEditor _textEditor;
        #endregion

        CommandBinding _undoBinding;
        CommandBinding _redoBinding;


        #region Constructors
        public CsvTextEditorService(TextEditor textEditor, ICommandManager commandManager)
        {
            Argument.IsNotNull(() => textEditor);
            Argument.IsNotNull(() => commandManager);

            _textEditor = textEditor;
            _commandManager = commandManager;

            _textEditor.TextArea.SelectionChanged += OnTextAreaSelectionChanged;
            _textEditor.TextChanged += OnTextChanged;

            AvalonEditCommands.DeleteLine.InputGestures.Add(new KeyGesture(Key.L, ModifierKeys.Control));
        //    ApplicationCommands.Undo
        }
        #endregion

        #region Properties
        public bool IsDirty { get; set; }
        public bool HasSelection => _textEditor.SelectionLength > 0;
        public bool CanRedo => _textEditor.CanRedo;
        public bool CanUndo => _textEditor.CanUndo;
        #endregion

        #region Methods
        public void Copy()
        {
            _textEditor.Copy();
        }

        public void Cut()
        {
            _textEditor.Cut();
        }

        public void Paste()
        {
            _textEditor.Paste();
        }

        public void Redo()
        {
            _textEditor.Redo();
        }

        public void Undo()
        {
            _textEditor.Undo();
        }

        private void OnTextAreaSelectionChanged(object sender, EventArgs e)
        {
            _commandManager.InvalidateCommands();
        }

        private void OnTextChanged(object sender, EventArgs e)
        {
            _commandManager.InvalidateCommands();

            if (_undoBinding == null)
            {
                _undoBinding = new CommandBinding(
                    ApplicationCommands.Undo, new ExecutedRoutedEventHandler(UndoExecuted), null);
                _redoBinding = new CommandBinding(
                    ApplicationCommands.Redo, new ExecutedRoutedEventHandler(RedoExecuted), null);

                _textEditor.CommandBindings.Add(_undoBinding);
                _textEditor.CommandBindings.Add(_redoBinding);
            }
        }

        private void UndoExecuted(object sender, ExecutedRoutedEventArgs args)
        {
          //  ApplicationCommands.Undo.Execute(null, Application.Current.MainWindow);
        }

        private void RedoExecuted(object sender, ExecutedRoutedEventArgs args)
        {
       //    ApplicationCommands.Redo.Execute(null, Application.Current.MainWindow);
        }
        #endregion
    }
}