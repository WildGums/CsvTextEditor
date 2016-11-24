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
        public static string InsertCommaSeparatedColumn(this string text, int column, int linesCount, int columnsCount, string newLine)
        {
            if (column == columnsCount)
            {
                return text.Replace(newLine, Symbols.Comma + newLine) + Symbols.Comma;
            }

            if (column == 0)
            {
                return text.Insert(0, Symbols.Comma.ToString())
                    .Replace(newLine, newLine + Symbols.Comma);
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

            return new string(textArray).TrimEnd(newLine);
        }

        public static string InsertLineWithTextTransfer(this string text, int insertLineIndex, int offsetInLine, int columnCount, string newLine)
        {
            var newLineLenght = newLine.Length;

            if (offsetInLine == 0 || insertLineIndex == 0)
            {
                return InsertLine(text, insertLineIndex, columnCount, newLine);
            }

            var previousLineOffset = insertLineIndex == 1 ? 0 : text.IndexOfSpecificOccurance(newLine, insertLineIndex - 1) + newLineLenght;
            var leftLineChunk = text.Substring(previousLineOffset, offsetInLine);
            var splitColumnIndex = leftLineChunk.Count(x => x.Equals(Symbols.Comma));

            var insetionText = $"{new string(Symbols.Comma, columnCount - splitColumnIndex - 1)}{newLine}{new string(Symbols.Comma, splitColumnIndex)}";

            var insertPosition = previousLineOffset + offsetInLine;
            return text.Insert(insertPosition, insetionText).TrimEnd(newLine);
        }

        public static string InsertLine(this string text, int insertLineIndex, int columnsCount, string newLine)
        {
            var newLineLenght = newLine.Length;

            var insertLineText = $"{new string(Symbols.Comma, columnsCount - 1)}{newLine}";
            var insertionPosition = insertLineIndex != 0 ? text.IndexOfSpecificOccurance(newLine, insertLineIndex) + newLineLenght : 0;

            return text.Insert(insertionPosition, insertLineText).TrimEnd(newLine);
        }

        public static string DuplicateTextInLine(this string csvText, int startOffset, int endOffset, string newLine)
        {
            var lineToDuplicate = csvText.Substring(startOffset, endOffset - startOffset);
            if (!lineToDuplicate.EndsWith(newLine))
            {
                lineToDuplicate = newLine + lineToDuplicate;
            }

            return csvText.Insert(endOffset, lineToDuplicate).TrimEnd(newLine);
        }

        public static string RemoveText(this string csvText, int startOffset, int endOffset, string newLine)
        {
            return csvText.Remove(startOffset, endOffset - startOffset).TrimEnd(newLine);
        }

        public static string RemoveCommaSeparatedColumn(this string text, int column, int linesCount, int columnsCount, string newLine)
        {
            if (columnsCount == 0 || linesCount == 0)
            {
                return string.Empty;
            }

            if (columnsCount == 1)
            {
                return string.Empty;
            }

            var newLineLenght = newLine.Length;

            var newCount = text.Length;
            var textArray = new char[newCount];
            var indexer = 0;

            var separatorCounter = 0;
            var isLastColumn = columnsCount - 1 == column;

            for (var i = 0; i < text.Length; i++)
            {
                var c = text[i];
                var isSeparator = false;

                if (c == Symbols.Comma)
                {
                    isSeparator = true;

                    if (separatorCounter == column)
                    {
                        separatorCounter++;
                        continue;
                    }
                    
                    separatorCounter++;

                    if (isLastColumn && separatorCounter == column)
                    {
                        continue;
                    }
                }
                else
                {
                    if(IsLookupMatch(text, i, newLine))
                    {
                        separatorCounter = 0;

                        i += newLineLenght - 1;
                        indexer = WriteStringToCharArray(textArray, newLine, indexer);

                        continue;
                    }
                }

                if (!isSeparator && separatorCounter == column)
                {
                    continue;
                }

                textArray[indexer] = c;
                indexer++;

                //if (c == Symbols.Comma)
                //{
                //    commaCounter++;

                //    if (commaCounter == column - 1)
                //    {
                //        continue;
                //    }
                //}
                
                //if (IsLookupMatch(text, i, newLine))
                //{
                //    commaCounter = 0;

                //    i += newLineLenght - 1;
                //    indexer = WriteStringToCharArray(textArray, newLine, indexer);

                //    continue;
                //}

                //if (commaCounter == column)
                //{
                //    continue;
                //}

                //textArray[indexer] = c;
                //indexer++;
            }

            return new string(textArray, 0, indexer).TrimEnd(newLine); 
        }

        public static string GetNewLineSymbol(this string text)
        {
            if (text.Contains(Environment.NewLine))
            {
                return Environment.NewLine;
            }
            else
            {
                if (text.Contains("\n"))
                {
                    return "\n";
                }
            }

            return Environment.NewLine;
        }

        public static string TrimEnd(this string target, string trimString)
        {
            var result = target;
            while (result.EndsWith(trimString))
            {
                result = result.Substring(0, result.Length - trimString.Length);
            }

            return result;
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

        private static int IndexOfSpecificOccurance(this string source, string value, int occuranceNumber)
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

        private static int WriteStringToCharArray(char[] array, string text, int startPosition)
        {
            var indexer = startPosition;

            foreach (var newLineChar in text)
            {
                array[indexer] = newLineChar;
                indexer++;
            }

            return indexer;
        }
    }
}