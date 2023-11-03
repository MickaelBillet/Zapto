using Microcharts;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Xamarin.Forms;

namespace AirZapto.Mobile.Converters
{
	public class FloatToIntegerOrFractionnalPart : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string result = string.Empty;

            if (value != null)
            {
                char[] cars = { '.', ',' };

                float floatValue = (float)value;

                string[] tmp = Math.Round(floatValue, 1).ToString("0.0").Split(cars);

                if (tmp != null)
                {
                    if (parameter.ToString().Contains("Integer") && (tmp.Length > 0))
                    {
                        result = tmp[0];
                    }
                    else if (parameter.ToString().Contains("Fractionnal") && (tmp.Length > 1))
                    {
                        result = tmp[1];
                    }
                }
            }

            return result;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }

    /// <summary>
    /// If int > 0 ? true : false
    /// </summary>
    public class LengthToIsVisibleConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
            int count = (int)value;

			if (count > 0)
			{
                return true;
			}

            return false;
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotSupportedException();
		}
	}

    public class OrderToBooleanConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {           
            int order = (int)value;

            if (order == 1)
            {
                return true;
            }

            return false;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if ((bool)value == true)
            {
                return 1;
            }

            return 0;
        }
    }

    public class UpperCaseConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return parameter.ToString().ToUpper();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }

    public class StringCaseConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string param = System.Convert.ToString(parameter) ?? "u";

            switch (param.ToUpper())
            {
                case "U":
                    return ((string)value).ToUpper();
                case "L":
                    return ((string)value).ToLower();
                default:
                    return ((string)value);
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }

    public class ToggledItemEventArgsConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            ToggledEventArgs eventArgs = value as ToggledEventArgs;

            return eventArgs.Value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class CheckedEventArgsConverter : IValueConverter
	{
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            CheckedChangedEventArgs eventArgs = value as CheckedChangedEventArgs;

            if (eventArgs == null)
                throw new ArgumentException("Expected EventArgs as value", "value");

            return eventArgs.Value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class ItemTappedEventArgsConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
            ItemTappedEventArgs eventArgs = value as ItemTappedEventArgs;

			if (eventArgs == null)
				throw new ArgumentException("Expected TappedEventArgs as value", "value");

			return eventArgs.Item;
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}

    public class StringToBoolConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (String.IsNullOrEmpty(value as String))
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class BooleanToBooleanConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			if ((Boolean)value == true)
			{
				return false;
			}
			else
			{
				return true;
			}
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}

	public class BooleanToOpacityConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			if ((Boolean)value == true)
			{
				return 1;
			}
			else
			{
                return 0.5;
			}
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();

		}
	}   

	public class BooleanToHeightConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			if ((Boolean)value == true)
			{
				return 0;
			}
			else
			{
				return 30;
			}
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}

    public class DurationConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            String res = String.Empty;

            if (value is double)
            {
                long seconds = (long)Math.Round((double)value, 0);

                if (seconds > 3600)
                {
                    return (Math.Round((double)(seconds / 3600), MidpointRounding.AwayFromZero) + " h " + Math.Round((double)((seconds % 3600)/60), MidpointRounding.AwayFromZero) + " min ");
                }
                else
                {
                    return Math.Round((double)((seconds) / 60), MidpointRounding.AwayFromZero) + " min ";
                }
            }

            return res;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class DecimalToStringIntConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
            if (value is decimal)
                return string.Format("{0:###0}", value);
			return null;
		}

		public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
			decimal dec;
			if (decimal.TryParse(value as string, out dec))
				return dec;
			return null;
		}
	}

    public class StringDecimalToStringIntConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Decimal valDec;

            bool res = Decimal.TryParse((value as String), out valDec);

            if (res == true)
            {
                return string.Format("{0:##0}", valDec);
            }

            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class DecimalToStringDecimalConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return string.Format( culture, "{0}", (Decimal)value);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class StringToIntConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
            Int32 dec;

            if (Int32.TryParse(value as string, out dec))
				return dec;
            
			return null;
		}

		public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
            if (value is Int32)
				return value.ToString();

            return null;
		}
	}

    public class IntToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if ((value is Int32) || (value is short))
                return value.ToString();

            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (Int32.TryParse(value as string, out int res0))
                return res0;
            else if (short.TryParse(value as string, out short res1))
                return res1;

            return 0;
        }
    }

    public class StringToDecimalConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
			Decimal dec;
			if (Decimal.TryParse(value as string, out dec))
				return dec;

			return null;
		}

		public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
			if (value is Decimal)
				return value.ToString();

			return null;
		}
	}

    public class FloatNullableToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value is float?)
                return value.ToString();

            return String.Empty;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        { 
            String val = (String)value;
            float? res = null;

            if (val != String.Empty)
            {
                try
                {
                    res = float.Parse(val);
                }
                catch (FormatException)
                {
                    return String.Empty;
                }
            }

            return res;
        }
    }

    public class NullToBooleanConverter : IValueConverter
	{
		public object Convert(object value, Type targetType,
							  object parameter, CultureInfo culture)
		{
            if (value == null)
            {
                return false;
            }
            else
            {
                return true;
            }
		}

		public object ConvertBack(object value, Type targetType,
								  object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}

    public class BooleanToObjectConverter<T> : IValueConverter
	{
		public T FalseObject { set; get; }

		public T TrueObject { set; get; }

		public object Convert(object value, Type targetType,
							  object parameter, CultureInfo culture)
		{
			return (bool)value ? this.TrueObject : this.FalseObject;
		}

		public object ConvertBack(object value, Type targetType,
								  object parameter, CultureInfo culture)
		{
			return ((T)value).Equals(this.TrueObject);
		}
	}

    public class DateTimeToStringUTCConverter : IValueConverter
    {
		public object Convert(object value, Type targetType,
							  object parameter, CultureInfo culture)
		{
            String param = parameter as String;

            if (value is DateTime)
            {
                return ((DateTime)((DateTime)value)).ToString(param);
            }
            else 
            {
                return String.Empty;
            }
		}

		public object ConvertBack(object value, Type targetType,
								  object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
    }
    
    public class DateTimeToStringLocalConverter : IValueConverter
    {
        public object Convert(object value, Type targetType,
                              object parameter, CultureInfo culture)
        {
            String param = parameter as String;

            if (value is DateTime)
            {
                return ((DateTime)((DateTime)value)).ToString(param);
            }
            else
            {
                return String.Empty;
            }
        }

        public object ConvertBack(object value, Type targetType,
                                  object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class ModeVisibilityConverter : IValueConverter
	{
        public object Convert(object value, Type targetType,
                              object parameter, CultureInfo culture)
        {
            short modeVisible = short.Parse(parameter.ToString());
            bool isVisible = false;
            int mode = (int)value;

            if (mode == modeVisible)
			{
                isVisible = true;
			}

            return isVisible;
        }

        public object ConvertBack(object value, Type targetType,
                                  object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class ChartVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType,
                              object parameter, CultureInfo culture)
        {
            bool isVisible = false;
            var entries = (IEnumerable<ChartEntry>)value;

            if (entries?.Count() > 0)
            {
                isVisible = true;
            }

            return isVisible;
        }

        public object ConvertBack(object value, Type targetType,
                                  object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class SensorVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType,
                              object parameter, CultureInfo culture)
        {
            int mode = (int)value;

            return mode;
        }

        public object ConvertBack(object value, Type targetType,
                                  object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
