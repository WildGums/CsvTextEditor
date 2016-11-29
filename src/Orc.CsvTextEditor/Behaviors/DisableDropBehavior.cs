// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DisableDropBehavior.cs" company="WildGums">
//   Copyright (c) 2008 - 2016 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.CsvTextEditor
{
    using Catel.Windows.Interactivity;
    using ICSharpCode.AvalonEdit;

    public class DisableDropBehavior : BehaviorBase<TextEditor>
    {
        protected override void OnAttached()
        {
            base.OnAttached();

            var textArea = AssociatedObject?.TextArea;
            if (textArea == null)
            {
                return;
            }

            textArea.AllowDrop = false;
        }
    }
}