// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DisableDropBehavior.cs" company="WildGums">
//   Copyright (c) 2008 - 2017 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.CsvTextEditor
{
    using Catel.Windows.Interactivity;
    using ICSharpCode.AvalonEdit;

    internal class DisableDropBehavior : BehaviorBase<TextEditor>
    {
        protected override void OnAssociatedObjectLoaded()
        {
            base.OnAssociatedObjectLoaded();

            var textArea = AssociatedObject?.TextArea;
            if (textArea == null)
            {
                return;
            }

            textArea.AllowDrop = false;
        }

        protected override void OnAssociatedObjectUnloaded()
        {
            base.OnAssociatedObjectUnloaded();

            var textArea = AssociatedObject?.TextArea;
            if (textArea == null)
            {
                return;
            }

            textArea.AllowDrop = true;
        }
    }
}