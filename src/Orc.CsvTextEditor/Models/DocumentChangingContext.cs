// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DocumentChangingContext.cs" company="WildGums">
//   Copyright (c) 2008 - 2016 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.CsvTextEditor
{
    using ICSharpCode.AvalonEdit;

    internal class DocumentChangingContext
    {
        public string OldText { get; set; }
        public TextEditor TextEditor { get; set; }

        public TabSpaceElementGenerator ElementGenerator { get; set; }

        public ColumnWidthCalculator ColumnWidthCalculator { get; set; }

        public string InsertedText { get; set; }
        public int Offset { get; set; }
    }
}