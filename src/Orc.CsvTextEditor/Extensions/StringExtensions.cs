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
            if (column == columnsCount)
            {
                var newLineSymbvol = Environment.NewLine;
                text = text.Replace(newLineSymbvol, Symbols.Comma + newLineSymbvol);
                return text;
            }

            if (column == 0)
            {
                text = text.Insert(0, Symbols.Comma.ToString());

                var newLineSymbvol = Environment.NewLine;
                return text.Replace(newLineSymbvol, newLineSymbvol + Symbols.Comma);
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
        #endregion
    }
}