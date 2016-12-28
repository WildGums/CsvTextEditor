// --------------------------------------------------------------------------------------------------------------------
// <copyright file="NUnitTest1.cs" company="WildGums">
//   Copyright (c) 2008 - 2016 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


 //using NUnit.Framework;

namespace Orc.CsvTextEditor.Tests
{
    using NUnit.Framework;

    [TestFixture]
    public class StringExtensionsFacts
    {
        [TestCase("this is\r\n trimEnd test\r\n\r\n", "\r\n", "this is\r\n trimEnd test")]
        [TestCase("this is\n trimEnd test\n\n", "\n", "this is\n trimEnd test")]
        [TestCase("\n\n\n\n", "\n", "")]
        public void CorrectlyTrimEndOfText(string text, string trimStr, string expectedResult)
        {
            var result = text.TrimEnd(trimStr);

            Assert.AreEqual(expectedResult, result);
        }

        [TestCase("here, some; words to test", 0, "here")]
        [TestCase("here, some; words to test", 5, "")]
        [TestCase("here, some; words to test", 26, "")]
        [TestCase("here, some; words to test", -1, "")]
        public void CorrectlyGetWordFromOffset(string text, int positionStart, string expectedResult)
        {
            var result = text.GetWordFromOffset(positionStart);

            Assert.AreEqual(expectedResult, result);
        }

        [TestCase("01,34,67,9\n12,34,56,78", 1, 4, "\n", "0,,67,9\n12,34,56,78")]
        [TestCase("01,34,67,9\n12,34,56,78", 0, 4, "\n", ",4,67,9\n12,34,56,78")]
        [TestCase("01,34,67,9\n12,34,56,78", 1, 14, "\n", "0,,,\n,4,56,78")]
        [TestCase("01,34,67,9\n12,34,56,78", 1, 21, "\n", "0,,,\n,,,")]
        public void RemoveCommaSeparatedTextCorrectlyRemovesTextFromGivenPositionAsItWasCsv(string text, int positionStart, int lenght, string newLine, string expectedResult)
        {
            var result = text.RemoveCommaSeparatedText(positionStart, lenght, newLine);

            Assert.AreEqual(expectedResult, result);
        }

        [TestCase("01,34,67,9\n12,34,56,78\n123,456,789,900", 0, 3, 4, "\n", ",01,34,67,9\n,12,34,56,78\n,123,456,789,900")]
        [TestCase("01,34,67,9\n12,34,56,78\n123,456,789,900", 4, 3, 4, "\n", "01,34,67,9,\n12,34,56,78,\n123,456,789,900,")]
        [TestCase("01,34,67,9\n12,34,56,78\n123,456,789,900", 2, 3, 4, "\n", "01,34,,67,9\n12,34,,56,78\n123,456,,789,900")]
        public void InsertCommaSeparatedColumnCorrectlyInsertsColumnInGivenPositionAsItWasCsv(string text, int column, int lineCount, int columnCount, string newLine, string expectedResult)
        {
            var result = text.InsertCommaSeparatedColumn(column, lineCount, columnCount, newLine);

            Assert.AreEqual(expectedResult, result);
        }

        [TestCase("01,34,67,9\n12,34,56,78\n123,456,789,900", 0, 0, 4, "\n", ",,,\n01,34,67,9\n12,34,56,78\n123,456,789,900")]
        [TestCase("01,34,67,9\n12,34,56,78\n123,456,789,900", 2, 4, 4, "\n", "01,34,67,9\n12,3,,\n,4,56,78\n123,456,789,900")]
        [TestCase("01,34,67,9\n12,34,56,78\n123,456,789,900", 3, 12, 4, "\n", "01,34,67,9\n12,34,56,78\n123,456,789,\n,,,900")]
        public void InsertLineWithTextTransferCorrectlyInsertsLineInGivenPositionAsItWasCsv(string text, int insertLineIndex, int offsetInLine, int columnCount, string newLine, string expectedResult)
        {
            var result = text.InsertLineWithTextTransfer(insertLineIndex, offsetInLine, columnCount, newLine);

            Assert.AreEqual(expectedResult, result);
        }

        [TestCase("01,34,67,9\n12,34,56,78\n123,456,789,900", 0, 11, "\n", "01,34,67,9\n01,34,67,9\n12,34,56,78\n123,456,789,900")]
        [TestCase("01,34,67,9\n12,34,56,78\n123,456,789,900", 11, 23, "\n", "01,34,67,9\n12,34,56,78\n12,34,56,78\n123,456,789,900")]
        [TestCase("01,34,67,9\n12,34,56,78\n123,456,789,900", 23, 38, "\n", "01,34,67,9\n12,34,56,78\n123,456,789,900\n123,456,789,900")]
        public void DuplicateTextInLineCorrectlyDuplicateTextBetweenGivenPositionsAsItWasCsv(string text, int startOffset, int endOffset, string newLine, string expectedResult)
        {
            var result = text.DuplicateTextInLine(startOffset, endOffset, newLine);

            Assert.AreEqual(expectedResult, result);
        }

        [TestCase("01,34,67,9\n12,34,56,78\n123,456,789,900", 0, 3, 4, "\n", "34,67,9\n34,56,78\n456,789,900")]
        [TestCase("01,34,67,9\n12,34,56,78\n123,456,789,900", 2, 3, 4, "\n", "01,34,9\n12,34,78\n123,456,900")]
        [TestCase("01,34,67,9\n12,34,56,78\n123,456,789,900", 3, 3, 4, "\n", "01,34,67\n12,34,56\n123,456,789")]
        public void RemoveColumnCorrectlyAsItWasCsv(string text, int column, int linesCount, int columnsCount, string newLine, string expectedResult)
        {
            var result = text.RemoveCommaSeparatedColumn(column, linesCount, columnsCount, newLine);

            Assert.AreEqual(expectedResult, result);
        }

        [TestCase("01,34,67,9\n12,34,56,78\n123,456,789,900", 0, 11, "\n", "12,34,56,78\n123,456,789,900")]
        [TestCase("01,34,67,9\n12,34,56,78\n123,456,789,900", 9, 12, "\n", "01,34,67,2,34,56,78\n123,456,789,900")]
        [TestCase("01,34,67,9\n12,34,56,78\n123,456,789,900", 15, 18, "\n", "01,34,67,9\n12,36,78\n123,456,789,900")]
        public void RemoveTextCorrectlyRemovesTextBetweenGivenPosition(string text, int startOffset, int endOffset, string newLine, string expectedResult)
        {
            var result = text.RemoveText(startOffset, endOffset, newLine);

            Assert.AreEqual(expectedResult, result);
        }

        [TestCase("01,34,67,9\r\n12,34,56,78\r\n123,456,789,900", "\r\n")]
        [TestCase("01,34,67,9\n12,34,56,78\n123,456,789,900", "\n")]
        [TestCase("01,34,67,9\n123,456,789,900", "\n")]
        public void GetNewLineSymbolCorrectlyAsItWasCsv(string text, string expectedResult)
        {
            var result = text.GetNewLineSymbol();

            Assert.AreEqual(expectedResult, result);
        }
    }
}