// --------------------------------------------------------------------------------------------------------------------
// <copyright file="HighlightAllOccurencesOfSelectedWordTransformer.cs">
//   Copyright http://stackoverflow.com/questions/9223674/highlight-all-occurrences-of-selected-word-in-avalonedit
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace Orc.CsvTextEditor.Transformers
{
    using System.Windows.Media;
    using ICSharpCode.AvalonEdit.Document;
    using ICSharpCode.AvalonEdit.Editing;
    using ICSharpCode.AvalonEdit.Rendering;

    public class HighlightAllOccurencesOfSelectedWordTransformer : DocumentColorizingTransformer
    {
        public string SelectedWord { private get; set; }
        public Selection Selection { private get; set; }

        protected override void ColorizeLine(DocumentLine line)
        {
            var lineStartOffset = line.Offset;
            var text = CurrentContext.Document.GetText(line);
            var start = 0;
            int index;

            if (string.IsNullOrEmpty(SelectedWord) || Selection == null)
            {
                return;
            }

            while ((index = text.IndexOf(SelectedWord, start)) >= 0)
            {
                // Don't highlight the current selection
                if (Selection.StartPosition.Column == index + 1 && Selection.StartPosition.Line == line.LineNumber)
                {
                    start = Selection.EndPosition.Column;
                    continue;
                }

                base.ChangeLinePart(
                    lineStartOffset + index, // startOffset
                    lineStartOffset + index + SelectedWord.Length, // endOffset
                    (VisualLineElement element) =>
                    {
                        element.TextRunProperties.SetBackgroundBrush(Brushes.PaleGreen);
                    });

                start = index + 1; // search for next occurrence
            }
        }
    }
}