using Avalonia.Data.Converters;
using System;
using System.Collections.Generic;
using System.Globalization;

namespace GOILauncher.Converters
{
    public class MultiplyConverter : IMultiValueConverter
    {
        public object Convert(IList<object?> values, Type targetType, object? parameter, CultureInfo culture)
        {
            double result = 1;
            foreach(var val in values)
            {
                if(val is double v)
                {
                    result *= v;
                }
            }
            return result;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
