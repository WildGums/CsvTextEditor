// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TabSpaceElementGenerator.cs" company="WildGums">
//   Copyright (c) 2008 - 2016 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.CsvTextEditor
{
    using ICSharpCode.AvalonEdit.Document;
    using ICSharpCode.AvalonEdit.Rendering;

    internal class TabSpaceElementGenerator : VisualLineElementGenerator
    {
        #region Fields
        private readonly ColumnWidthCalculator _columnWidthCalculator;

        private int _tabWidth;
        public int[] ColumnWidth;
        public int[][] Lines;
        #endregion

        #region Constructors
        public TabSpaceElementGenerator(ColumnWidthCalculator columnWidthCalculator)
        {
            _columnWidthCalculator = columnWidthCalculator;
        }
        #endregion

        #region Methods
        public override VisualLineElement ConstructElement(int offset)
        {
            if (CurrentContext.VisualLine.LastDocumentLine.EndOffset == offset)
            {
                return null;
            }

            return new EmptyVisualLineElement(_tabWidth + 1, 1);
        }

        public override int GetFirstInterestedOffset(int startOffset)
        {
            if (Lines == null)
            {
                return startOffset;
            }

            var location = CurrentContext.Document.GetLocation(startOffset);

            var columnNumberWithOffset = _columnWidthCalculator.GetColumn(Lines, location);
            var locationLine = location.Line;

            if (columnNumberWithOffset.ColumnNumber == ColumnWidth.Length - 1)
            {
                if (Lines.Length == locationLine)
                {
                    return CurrentContext.VisualLine.LastDocumentLine.EndOffset;
                }

                columnNumberWithOffset = new ColumnNumberWithOffset
                {
                    ColumnNumber = 0,
                    OffsetInLine = Lines[locationLine][0]
                };

                locationLine++;
            }

            _tabWidth = ColumnWidth[columnNumberWithOffset.ColumnNumber] -
                        Lines[locationLine - 1][columnNumberWithOffset.ColumnNumber];

            return CurrentContext.Document.GetOffset(new TextLocation(locationLine, columnNumberWithOffset.OffsetInLine));
        }
        #endregion
    }
}