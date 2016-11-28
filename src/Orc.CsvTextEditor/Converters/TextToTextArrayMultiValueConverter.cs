// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TextToTextArrayMultiValueConverter.cs" company="WildGums">
//   Copyright (c) 2008 - 2016 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.CsvTextEditor.Converters
{
    using System;
    using System.Globalization;
    using System.Windows.Data;

    public class TextToTextArrayMultiValueConverter : IMultiValueConverter
    {
        #region Methods
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            return values.Clone();
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}