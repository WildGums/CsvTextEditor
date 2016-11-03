// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CsvTextHelper.cs" company="WildGums">
//   Copyright (c) 2008 - 2016 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.CsvTextEditor.Helpers
{
    public static class CsvTextHelper
    {
        public static string InsertColumn(string text, int column, int lineCount, int columnCount, char csvColumnDelimeter = ',')
        {
            var commaCounter = 0;

            var newCount = text.Length + lineCount - 1;
            var textArray = new char[newCount];
            var indexer = 0;
            foreach (var c in text)
            {
                if (c == csvColumnDelimeter)
                {
                    if (commaCounter == column)
                    {
                        textArray[indexer] = csvColumnDelimeter;
                        indexer++;
                    }

                    if (commaCounter == columnCount - 1)
                    {
                        commaCounter = 0;
                    }

                    commaCounter++;
                }

                textArray[indexer] = c;
                indexer++;
            }

            return new string(textArray);
        }
    }
}