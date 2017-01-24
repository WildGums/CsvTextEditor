// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ColumnNumberWithOffset.cs" company="WildGums">
//   Copyright (c) 2008 - 2017 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.CsvTextEditor
{
    internal struct ColumnNumberWithOffset
    {
        public int ColumnNumber;
        public int OffsetInLine;
        public int Length;
    }
}