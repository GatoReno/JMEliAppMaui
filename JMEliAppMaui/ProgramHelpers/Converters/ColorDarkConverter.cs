using System;
using System.Globalization;

namespace JMEliAppMaui.ProgramHelpers.Converters
{
    public class ColorDarkConverter : IValueConverter
    {
		 

        public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            if (value != null)
            {
                switch (value)
                {
                    case "IndianRed":
                        return "DarkRed";
                         
                    case "Green":
                        return "DarkGreen";
                     case "Gold":
                        return "DarkGoldenrod";
                     default:
                        return value;
                }
            }
            
            else return value;
        }

        public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

