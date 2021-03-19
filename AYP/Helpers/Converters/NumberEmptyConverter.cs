using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Windows.Data;

namespace AYP.Helpers.Converters
{
    public class NumberEmptyConverter: IValueConverter
    {
		public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
            string response = "";

            if (value is int)
            {
                int number = (int)value;

                if (number == 0)
                {
                    response = number.ToString(" ");
                }
                else
                {
                    response = number.ToString();
                }
            }
            else if(value is decimal)
            {
                decimal number = (decimal)value;

                if (number == 0)
                {
                    response = number.ToString(" ");
                }
                else
                {
                    response = number.ToString();
                }
            }

            return response;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            int response = 0;

            if (string.IsNullOrEmpty(value.ToString()))
            {
                response = System.Convert.ToInt32(value.ToString());
            }

            return response;
        }
    }
}
