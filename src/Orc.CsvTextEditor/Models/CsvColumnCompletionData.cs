// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CsvColumnCompletionData.cs" company="WildGums">
//   Copyright (c) 2008 - 2017 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.CsvTextEditor
{
    using System;
    using ICSharpCode.AvalonEdit.CodeCompletion;
    using ICSharpCode.AvalonEdit.Document;
    using ICSharpCode.AvalonEdit.Editing;

    public class CsvColumnCompletionData : ICompletionData
    {
        #region Constructors
        public CsvColumnCompletionData(string text)
        {
            Text = text;
        }
        #endregion

        #region Properties
        public System.Windows.Media.ImageSource Image => null;

        public string Text { get; }
        public object Content => Text;
        public object Description => Text;
        public double Priority { get; }
        #endregion

        #region Methods
        public void Complete(TextArea textArea, ISegment completionSegment,
            EventArgs insertionRequestEventArgs)
        {
            textArea.Document.Replace(completionSegment, Text);
        }
        #endregion
    }
}