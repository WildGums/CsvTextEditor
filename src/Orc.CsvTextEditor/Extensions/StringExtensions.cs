// --------------------------------------------------------------------------------------------------------------------
// <copyright file="StringExtensions.cs" company="WildGums">
//   Copyright (c) 2008 - 2016 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.CsvTextEditor
{
    using System;

    public static class StringExtensions
    {
        #region Methods
        public static string InsertCommaSeparatedColumn(this string text, int column, int linesCount, int columnsCount)
        {
            var newCount = text.Length + linesCount;
            var textArray = new char[newCount];
            var indexer = 0;

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
        #endregion
    }
}