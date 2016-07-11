using ICSharpCode.AvalonEdit.Document;
using ICSharpCode.AvalonEdit.Rendering;

namespace CsvTextEditor.Control
{
    internal struct ColumnNumberWithOffset
    {
        public int ColumnNumber;
        public int OffsetInLine;
    }

    internal class ColumnWidthCalculator
    {
        public ColumnNumberWithOffset GetColumn(int[][] lines, TextLocation location)
        {
            var currentLineIndex = location.Line - 1;
            var currentColumnIndex = location.Column - 1;

            var currentLine = lines[currentLineIndex];

            var sum = 0;
            var i = 0;
            while (currentLine.Length > i && sum <= currentColumnIndex)
            {
                sum += currentLine[i];
                i++;
            }
            i--;

            return new ColumnNumberWithOffset {ColumnNumber = i, OffsetInLine = sum};
        }
    }

    internal class TabSpaceElementGenerator : VisualLineElementGenerator
    {
        private readonly ColumnWidthCalculator _columnWidthCalculator;

        private int _tabWidth;
        public int[] ColumnWidth;
        public int[][] Lines;

        public TabSpaceElementGenerator(ColumnWidthCalculator columnWidthCalculator)
        {
            _columnWidthCalculator = columnWidthCalculator;
        }

        public override VisualLineElement ConstructElement(int offset)
        {
            if (CurrentContext.VisualLine.LastDocumentLine.EndOffset == offset) return null;

            return new EmptyVisualLineElement(_tabWidth + 1, 1);
        }

        public override int GetFirstInterestedOffset(int startOffset)
        {
            if (Lines == null)
                return startOffset;

            var location = CurrentContext.Document.GetLocation(startOffset);

            var columnNumberWithOffset = _columnWidthCalculator.GetColumn(Lines, location);
            var locationLine = location.Line;

            if (columnNumberWithOffset.ColumnNumber == ColumnWidth.Length - 1)
            {
                if (Lines.Length == locationLine)
                {
                    return CurrentContext.VisualLine.LastDocumentLine.EndOffset;
                }
                else
                {
                    columnNumberWithOffset = new ColumnNumberWithOffset
                    {
                        ColumnNumber = 0,
                        OffsetInLine = Lines[locationLine][0]
                    };
                    locationLine++;
                }
            }

            _tabWidth = ColumnWidth[columnNumberWithOffset.ColumnNumber] -
                        Lines[locationLine - 1][columnNumberWithOffset.ColumnNumber];

            return CurrentContext.Document.GetOffset(new TextLocation(locationLine, columnNumberWithOffset.OffsetInLine));
        }
    }
}