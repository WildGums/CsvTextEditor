// --------------------------------------------------------------------------------------------------------------------
// <copyright file="StringExtensions.cs" company="WildGums">
//   Copyright (c) 2008 - 2016 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.CsvTextEditor
{
    public static class StringExtensions
    {
        #region Methods
        public static string InsertCommaSeparatedColumn(this string text, int column, int linesCount, int columnsCount)
        {
            var commaCounter = 0;

            var newCount = text.Length + linesCount;
            var textArray = new char[newCount];
            var indexer = 0;
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

                    if (commaCounter == columnsCount-1)
                    {
                        commaCounter = 0;
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