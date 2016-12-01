// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TextEditorSynchronizeTextBehavior.cs" company="WildGums">
//   Copyright (c) 2008 - 2016 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.CsvTextEditor
{
    using System;
    using System.ComponentModel;
    using Catel.IoC;
    using Catel.Windows.Interactivity;
    using Services;

    internal class TextEditorSynchronizeTextBehavior : BehaviorBase<CsvTextEditorControl>
    {
        #region Fields
        private ICsvTextSynchronizationService _csvTextSynchronizationService;
        #endregion

        protected override void OnAssociatedObjectLoaded()
        {
            base.OnAssociatedObjectLoaded();

            var textEditorControl = AssociatedObject;

            textEditorControl.Initialized += OnInitialized;
            textEditorControl.PropertyChanged += OnTextEditorControlPropertyChanged;
        }

        protected override void OnAssociatedObjectUnloaded()
        {
            var textEditorControl = AssociatedObject;

            _csvTextSynchronizationService = null;
            textEditorControl.Initialized -= OnInitialized;
            textEditorControl.PropertyChanged -= OnTextEditorControlPropertyChanged;

            var textEditor = AssociatedObject?.TextEditor;
            if (textEditor == null)
            {
                return;
            }
            
            textEditor.TextChanged -= OnTextChanged;

            base.OnAssociatedObjectUnloaded();
        }

        private void OnTextChanged(object sender, EventArgs eventArgs)
        {
            var textEditor = AssociatedObject?.TextEditor;
            if (textEditor == null)
            {
                return;
            }

            using (_csvTextSynchronizationService.SynchronizeInScope())
            {
                AssociatedObject.Text = textEditor.Text;
            }
        }

        private void OnInitialized(object sender, EventArgs eventArgs)
        {
            var textEditor = AssociatedObject?.TextEditor;
            if (textEditor == null)
            {
                return;
            }

            textEditor.TextChanged += OnTextChanged;
        }

        private void OnTextEditorControlPropertyChanged(object sender, PropertyChangedEventArgs args)
        {
            var textEditorControl = AssociatedObject;
            if (args.PropertyName != nameof(textEditorControl.Scope))
            {
                return;
            }

            var serviceLocator = this.GetServiceLocator();
            var scope = textEditorControl.Scope;

            _csvTextSynchronizationService = serviceLocator.ResolveType<ICsvTextSynchronizationService>(scope);
        }
    }
}