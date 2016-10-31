// --------------------------------------------------------------------------------------------------------------------
// <copyright file="EmptyVisualLineElement.cs" company="WildGums">
//   Copyright (c) 2008 - 2016 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.CsvTextEditor
{
    using System.Windows.Media;
    using System.Windows.Media.TextFormatting;
    using ICSharpCode.AvalonEdit.Rendering;

    internal class EmptyVisualLineElement : VisualLineElement
    {
        #region Constructors
        public EmptyVisualLineElement(int visualLength, int documentLength)
            : base(visualLength, documentLength)
        {
        }
        #endregion

        #region Methods
        public override TextRun CreateTextRun(int startVisualColumn, ITextRunConstructionContext context)
        {
            TextRunProperties.SetBackgroundBrush(new SolidColorBrush(Colors.LightCyan));

            return new TextCharacters(" ", TextRunProperties);
        }

        public override bool IsWhitespace(int visualColumn)
        {
            return true;
        }
        #endregion
    }
}