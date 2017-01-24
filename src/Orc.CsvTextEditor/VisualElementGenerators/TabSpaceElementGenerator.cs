// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TabSpaceElementGenerator.cs" company="WildGums">
//   Copyright (c) 2008 - 2017 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.CsvTextEditor
{
    using System;
    using System.Linq;
    using ICSharpCode.AvalonEdit.Document;
    using ICSharpCode.AvalonEdit.Rendering;

    internal class TabSpaceElementGenerator : VisualLineElementGenerator
    {
        #region Fields
        private int[][] _lines;
        private int _tabWidth;
        #endregion

        #region Properties
        public int[][] Lines
        {
            get { return _lines; }
            private set
            {
                if (Equals(value, _lines))
                {
                    return;
                }

                _lines = value;
                ColumnWidth = CalculateColumnWidth(_lines);
            }
        }

        public int[] ColumnWidth { get; private set; }
        public int ColumnCount => Lines[0].Length;
        public string NewLine { get; private set; }
        #endregion

        public void Refresh(string text)
        {
            text = text ?? string.Empty;

            NewLine = text.GetNewLineSymbol();
            
            // Some files do not respect the Environment.NewLine so we need to add "\n"
            var lines = text.Split(new[] { NewLine }, StringSplitOptions.None);

            var columnWidthByLine = lines.Select(x => x.Split(Symbols.Comma))
                .Select(x => x.Select(y => y.Length + 1).ToArray())
                .ToArray();

            Lines = columnWidthByLine;
        }

        public bool RefreshLocation(TextLocation affectedLocation, int length)
        {
            var columnWidth = ColumnWidth;
            var columnWidthByLine = Lines;

            var columnNumberWithOffset = GetColumn(affectedLocation);

            var affectedColumn = columnNumberWithOffset.ColumnNumber;
            var affectedLine = affectedLocation.Line - 1;
            var oldWidth = columnWidthByLine[affectedLine][affectedColumn];

            var newWidth = oldWidth + length;

            columnWidthByLine[affectedLine][affectedColumn] = newWidth;
            
            if (length >= 0 && columnWidth[affectedColumn] <= newWidth)
            {
                columnWidth[affectedColumn] = newWidth;
                return true;
            }

            if (length <= 0 && columnWidth[affectedColumn] >= newWidth)
            {
                columnWidth[affectedColumn] = columnWidthByLine.Where(x => x.Length > affectedColumn).Select(x => x[affectedColumn]).Max();
                return true;
            }
            
            return false;
        }

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

            var columnNumberWithOffset = GetColumn(location);
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

        public ColumnNumberWithOffset GetColumn(TextLocation location)
        {
            var lines = Lines;

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
                ColumnNumber = i - 1,
                OffsetInLine = sum,
                Length = currentLine[i - 1]
            };

            return column;
        }

        private int[] CalculateColumnWidth(int[][] columnWidthByLine)
        {
            if (columnWidthByLine.Length == 0)
            {
                return new int[0];
            }

            var accum = new int[columnWidthByLine[0].Length];

            foreach (var line in columnWidthByLine)
            {
                if (line.Length > accum.Length)
                {
                    throw new ArgumentException("Records in CSV have to contain the same number of fields");
                }

                var length = Math.Min(accum.Length, line.Length);

                for (var i = 0; i < length; i++)
                {
                    accum[i] = Math.Max(accum[i], line[i]);
                }
            }

            return accum.ToArray();
        }
    }
}