// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CaretTextLocationChangedEventArgs.cs" company="WildGums">
//   Copyright (c) 2008 - 2017 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.CsvTextEditor
{
    using System;

    public class CaretTextLocationChangedEventArgs : EventArgs
    {
        #region Constructors
        public CaretTextLocationChangedEventArgs(int column, int line)
        {
            Column = column;
            Line = line;
        }
        #endregion

        #region Properties
        public int Column { get; }
        public int Line { get; }
        #endregion
    }
}