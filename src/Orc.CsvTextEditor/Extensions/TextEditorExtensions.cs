// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TextEditorExtensions.cs" company="WildGums">
//   Copyright (c) 2008 - 2016 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.CsvTextEditor
{
    using System.Linq;
    using Catel;
    using ICSharpCode.AvalonEdit;

    public static class TextEditorExtensions
    {
        public static void SetCaretToSpecificLineAndColumn(this TextEditor textEditor, int lineIndex, int columnIndex, int[][] columnWidthByLine)
        {
            Argument.IsNotNull(() => textEditor);

            if (lineIndex < 0 || columnIndex < 0)
            {
                return;
            }

            var textDocument = textEditor.Document;

            var line = textDocument.Lines[lineIndex];
            var offset = line.Offset;
            var columnOffset = columnWidthByLine[lineIndex].Take(columnIndex).Sum();

            textEditor.CaretOffset = offset + columnOffset;
        }
    }
}