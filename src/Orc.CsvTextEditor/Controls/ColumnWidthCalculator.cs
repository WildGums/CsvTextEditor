// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ColumnWidthCalculator.cs" company="WildGums">
//   Copyright (c) 2008 - 2016 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.CsvTextEditor
{
    using ICSharpCode.AvalonEdit.Document;

    internal class ColumnWidthCalculator
    {
        #region Methods
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

            var column = new ColumnNumberWithOffset
            {
                ColumnNumber = i-1,
                OffsetInLine = sum
            };

            return column;
        }
        #endregion
    }
}