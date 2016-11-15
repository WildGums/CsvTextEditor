// --------------------------------------------------------------------------------------------------------------------
// <copyright file="StringExtensions.cs" company="WildGums">
//   Copyright (c) 2008 - 2016 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.CsvTextEditor
{
    using System;
    using System.Linq;

    public static class StringExtensions
    {
        #region Methods
        public static string InsertCommaSeparatedColumn(this string text, int column, int linesCount, int columnsCount)
        {
            var newLineSymbvol = Environment.NewLine;

            if (column == columnsCount)
            {
                return text.Replace(newLineSymbvol, Symbols.Comma + newLineSymbvol) + Symbols.Comma;
            }

            if (column == 0)
            {
                return text.Insert(0, Symbols.Comma.ToString())
                    .Replace(newLineSymbvol, newLineSymbvol + Symbols.Comma);
            }

            var newCount = text.Length + linesCount;
            var textArray = new char[newCount];
            var indexer = 0;

            var commaCounter = 1;
            foreach (var c in text)
            {
                if (c == Symbols.Comma)
                {
                    if (commaCounter == column)
                    {
                        textArray[indexer] = Symbols.Comma;
                        indexer++;
                    }

                    commaCounter++;

                    if (commaCounter == columnsCount)
                    {
                        commaCounter = 1;
                    }
                }

                textArray[indexer] = c;
                indexer++;
            }

            return new string(textArray);
        }

        public static string InsertLineWithTextTransfer(this string text, int insertLineIndex, int offsetInLine, int columnCount)
        {
            var newLine = Environment.NewLine;
            var newLineLenght = newLine.Length;

            if (offsetInLine == 0 || insertLineIndex == 0)
            {
                return InsertLine(text, insertLineIndex, columnCount);
            }

            var previousLineOffset = text.IndexOfSpecificOccurance(newLine, insertLineIndex - 1) + newLineLenght;
            var leftLineChunk = text.Substring(previousLineOffset, offsetInLine);
            var splitColumnIndex = leftLineChunk.Count(x => x.Equals(Symbols.Comma));

            var insetionText = $"{new string(Symbols.Comma, columnCount - splitColumnIndex - 1)}{newLine}{new string(Symbols.Comma, splitColumnIndex)}";

            var insertPosition = previousLineOffset + offsetInLine;
            text = text.Insert(insertPosition, insetionText);
            return text;
        }

        public static string InsertLine(this string text, int insertLineIndex, int columnsCount)
        {
            var newLine = Environment.NewLine;
            var newLineLenght = newLine.Length;

            var insertLineText = $"{new string(Symbols.Comma, columnsCount - 1)}{newLine}";
            var insertionPosition = insertLineIndex != 0 ? text.IndexOfSpecificOccurance(newLine, insertLineIndex) + newLineLenght : 0;

            return text.Insert(insertionPosition, insertLineText);
        }

        public static int IndexOfSpecificOccurance(this string source, string value, int occuranceNumber)
        {
            var index = -1;
            for (var i = 0; i < occuranceNumber; i++)
            {
                index = source.IndexOf(value, index + 1, StringComparison.Ordinal);

                if (index == -1)
                {
                    break;
                }
            }

            return index;
        }

        public static string DuplicateTextInLine(this string csvText, int startOffset, int endOffset)
        {
            var lineToDuplicate = csvText.Substring(startOffset, endOffset - startOffset);
            if (!lineToDuplicate.EndsWith(Environment.NewLine))
            {
                lineToDuplicate = Environment.NewLine + lineToDuplicate;
            }

            return csvText.Insert(endOffset, lineToDuplicate);
        }

        public static string RemoveCommaSeparatedColumn(this string text, int column, int linesCount, int columnsCount)
        {
            if (columnsCount == 0 || linesCount == 0)
            {
                return string.Empty;
            }

            var newLine = Environment.NewLine;
            var newLineLenght = newLine.Length;

            var newCount = text.Length;
            var textArray = new char[newCount];

            var commaCounter = 0;
            var indexer = 0;

            for (var i = 0; i < text.Length; i++)
            {
                var c = text[i];

                if (c == Symbols.Comma)
                {
                    commaCounter++;
                }

                if (IsLookupMatch(text, i, newLine))
                {
                    commaCounter = 0;

                    i += newLineLenght - 1;

                    foreach (var newLineChar in newLine)
                    {
                        textArray[indexer] = newLineChar;
                        indexer++;
                    }

                    continue;
                }

                if (commaCounter == column)
                {
                    continue;
                }

                textArray[indexer] = c;
                indexer++;
            }
            
            return new string(textArray, 0, indexer);
        }

        private static bool IsLookupMatch(string text, int startIndex, string lookup)
        {
            var lookupLength = lookup.Length;
            if (text.Length - startIndex < lookupLength)
            {
                return false;
            }

            var lookupNewLine = text.Substring(startIndex, lookupLength);
            return string.Equals(lookupNewLine, lookup);
        }
        #endregion
    }
}