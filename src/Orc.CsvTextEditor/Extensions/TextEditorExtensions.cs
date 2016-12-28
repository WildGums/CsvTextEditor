// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TextEditorExtensions.cs" company="WildGums">
//   Copyright (c) 2008 - 2016 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.CsvTextEditor
{
    using System.Collections.Generic;
    using System.Linq;
    using Catel;
    using ICSharpCode.AvalonEdit;
    using ICSharpCode.AvalonEdit.CodeCompletion;

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

            var lines = textDocument.Lines;
            if (lines.Count <= lineIndex)
            {
                return;
            }

            var line = textDocument.Lines[lineIndex];
            var offset = line.Offset;
            var columnOffset = columnWidthByLine[lineIndex].Take(columnIndex).Sum();

            var maxCaretOffset = textDocument.TextLength;
            var newCaretOffset = offset + columnOffset;
            textEditor.CaretOffset = maxCaretOffset > newCaretOffset ? newCaretOffset : maxCaretOffset;
        }

        public static IList<ICompletionData> GetCompletionDataForText(this TextEditor textEditor, string autocompletionText, int columnIndex, int[][] scheme)
        {
            Argument.IsNotNull(() => textEditor);

            var textDocument = textEditor.Document;
            var offset = textEditor.CaretOffset;

            var text = textEditor.Text;
            var lines = textDocument.Lines;

            var data = new SortedList<string, ICompletionData>();
            autocompletionText = autocompletionText.ToLower();
            
            for (var i = 1; i < lines.Count; i++)
            {
                var line = lines[i];
                if (line.Length == 0)
                {
                    continue;
                }

                var lineScheme = scheme[i];

                var columnWidth = lineScheme[columnIndex] - 1;
                var columnStart = lineScheme.Take(columnIndex).Sum();

                var lineOffset = line.Offset;
                var columnOffset = lineOffset + columnStart;

                var columnChunk = text.Substring(columnOffset, columnWidth);
                var words = columnChunk.Split(null);
                var currentWord = text.GetWordFromOffset(offset - 1);

                foreach (var word in words)
                {
                    if (string.Equals(currentWord, word))
                    {
                        continue;
                    }

                    if (data.ContainsKey(word))
                    {
                        continue;
                    }

                    if (word.ToLower().StartsWith(autocompletionText))
                    {
                        data.Add(word, new CsvColumnCompletionData(word));
                    }
                }
            }

            return data.Values;
        }
    }
}