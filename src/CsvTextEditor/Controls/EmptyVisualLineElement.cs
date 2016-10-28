using System.Windows.Media;
using System.Windows.Media.TextFormatting;
using ICSharpCode.AvalonEdit.Rendering;

namespace CsvTextEditor.Controls
{
    internal class EmptyVisualLineElement : VisualLineElement
    {
        public EmptyVisualLineElement(int visualLength, int documentLength)
            : base(visualLength, documentLength)
        {
        }

        public override TextRun CreateTextRun(int startVisualColumn, ITextRunConstructionContext context)
        {
            TextRunProperties.SetBackgroundBrush(new SolidColorBrush(Colors.LightCyan));
            return new TextCharacters(" ", TextRunProperties);
        }

        public override bool IsWhitespace(int visualColumn)
        {
            return true;
        }
    }
}