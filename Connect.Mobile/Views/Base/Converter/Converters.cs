using Connect.Mobile.Resources;
using Connect.Model;
using System;
using System.Globalization;
using System.Text;
using Xamarin.Forms;
using Condition = Connect.Model.Condition;

namespace Connect.Mobile.Converters
{
    public class DoubleToIntegerOrFractionnalPart : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string result = string.Empty;

            if (value != null)
            {
                char[] cars = { '.', ',' };

                double floatValue = (double)value;

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

    public class OperationRangeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
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

    public class ConditionTypeToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        { 
            int conditionType = (int)value;
            Boolean res = false;

            if (((String)parameter).Contains("Temperature"))
            {
                if ((conditionType & ParameterType.Temperature) == (ParameterType.Temperature))
                {
                    res = true;
                }
            }
            else if (((String)parameter).Contains("Humidity"))
            {
                if ((conditionType & ParameterType.Humidity) == (ParameterType.Humidity))
                {
                    res = true;
                }
            }

            return res;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class DeviceTypeToVisibilityConverter2 : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            int deviceType = (int)value;
            Boolean res = false;

            if (deviceType != DeviceType.None)
            {
                res = true;
            } 

            return res;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }

    public class DeviceTypeToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            int deviceType = (int)value;
            bool res = false;

            if (!string.IsNullOrEmpty(parameter as string))
            {
                if (((string)parameter).Contains("Temperature"))
                {
                    if ((deviceType & DeviceType.Sensor_Temperature) == DeviceType.Sensor_Temperature)
                    {
                        res = true;
                    }
                }

                if (((string)parameter).Contains("Pressure"))
                {
                    if ((deviceType & DeviceType.Sensor_Pressure) == DeviceType.Sensor_Pressure)
                    {
                        res = true;
                    }
                }

                if (((string)parameter).Contains("Humidity"))
                {
                    if ((deviceType & DeviceType.Sensor_Humidity) == DeviceType.Sensor_Humidity)
                    {
                        res = true;
                    }
                }
            }

            return res;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }

    /// <summary>
    /// parameter => String upper or lower
    /// </summary>
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

    /// <summary>
    /// parameter == value ? true : false
    /// </summary>
    public class StatusPlugConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Boolean res = false;
            Plug plug = value as Plug;
            if (plug != null)
            {
                if ((string)parameter == "LightningYellow")
                {
                    res = Plug.GetStatus(plug.Status, plug.Order) == Status.ON;
                }
                else if ((string)parameter == "LightningBlue")
                {
                    res = Plug.GetStatus(plug.Status, plug.Order) == Status.OffON;
                }
                else if ((string)parameter == "LightningBlueEmpty")
                {
                    res = Plug.GetStatus(plug.Status, plug.Order) == Status.OnOFF;
                }
            }
            return res;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotSupportedException();
		}
	}

    public class StatusToOpacityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            double opacity = 0.35;

            Plug plug = value as Plug;

            if (plug != null)
            {
                if ((plug.Status == Model.Status.ON) && (plug.Mode == Model.Mode.Manual))
                {
                    opacity = 1;
                }
            }

            return opacity;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class TemperatureOrderConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Boolean res = false;

            Condition condition = value as Condition;

            if (condition != null)
            {
                if ((condition.TemperatureOrderIsEnabled == 1) && (condition.TemperatureOrder != null))
                {
                    res = true;
                }
            }

            return res;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }

    public class HumidityOrderConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Boolean res = false;

            Condition condition = value as Condition;

            if (condition != null)
            {
                if ((condition.HumidityOrderIsEnabled == 1) && (condition.HumidityOrder != null))
                {
                    res = true;
                }
            }

            return res;
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

    public class NotificationConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            StringBuilder @string = new StringBuilder();

            if (value != null)
            {
                Notification notification = value as Notification;

                if (notification.Parameter == ParameterType.Humidity)
                {
                    @string.Append(AppResources.Humidity);
                }
                else if (notification.Parameter == ParameterType.Temperature)
                {
                    @string.Append(AppResources.Temperature);
                }

                if (notification.Sign == SignType.Upper)
                {
                    @string.Append(" > ");
                }
                else if (notification.Sign == SignType.Lower)
                {
                    @string.Append(" < ");
                }

                @string.Append(notification.Value);

                if (notification.Parameter == ParameterType.Humidity)
                {
                    @string.Append("%");
                }
                else if (notification.Parameter == ParameterType.Temperature)
                {
                    @string.Append("°C");
                }
            }

            return @string.ToString();
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();                
        }
    }

    public class PlugTypeToColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            ConnectedObject obj = value as ConnectedObject;

            Color color = (Color)Xamarin.Forms.Application.Current.Resources["StandardPlug"];

            if ((obj != null) && (obj.Plug != null))
            {
                if ((obj.Plug.ConditionType & ParameterType.Humidity) == (ParameterType.Humidity))
                {
                    color = (Color)Xamarin.Forms.Application.Current.Resources["HumidityPlug"];
                }
                else if ((obj.Plug.ConditionType & ParameterType.Temperature) == (ParameterType.Temperature))
                {
                    color = (Color)Xamarin.Forms.Application.Current.Resources["TemperaturePlug"];
                }
                else if ((obj.Plug.ConditionType & ParameterType.Humidity) == (ParameterType.Humidity)
                            && ((obj.Plug.ConditionType & ParameterType.Temperature) == (ParameterType.Temperature)))
                {
                    color = (Color)Xamarin.Forms.Application.Current.Resources["HumidityTemperaturePlug"];
                }
            }

            return color;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
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
    public class ParamImageConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            short param = (short)value;

            ImageSource source = null;

            if (param == ParameterType.Temperature)
            {
                source = ConvertToImageSource("icons8temperature40.png");
            }
            else if (param == ParameterType.Humidity)
            {
                source = ConvertToImageSource("icons8wet40.png");
            }

            return source;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        private ImageSource ConvertToImageSource(string fileName)
        {
            return Device.OnPlatform(
                 iOS: ImageSource.FromFile($"Images/{fileName}"),
                 Android: ImageSource.FromFile(fileName),
                 WinPhone: ImageSource.FromFile($"Images/{fileName}"));
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

    public class ParameterConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            short param = (short)value;

            bool res = false;

            if ((param == ParameterType.Temperature) && (parameter as string).Contains("Temperature"))
            {
                res = true;
            }
            else if ((param == ParameterType.Humidity) && (parameter as string).Contains("Humidity"))
            {
                res = true;
            }

            return res;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool res = (bool)value;
            short param = ParameterType.None;

            if ((res == true) && (parameter as string).Contains("Temperature")
                || (res == false) && (parameter as string).Contains("Humidity"))
            {
                param = ParameterType.Temperature;
            }
            else if ((res == true) && (parameter as string).Contains("Humidity")
                || (res == false) && (parameter as string).Contains("Temperature"))
            {
                param = ParameterType.Humidity;
            }

            return param;
        }
    }

    public class SignConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            short sign = (short)value;

            bool res = false;

            if ((sign == SignType.Lower) && (parameter as string).Contains("Lower"))
            {
                res = true;
            }
            else if ((sign == SignType.Upper) && (parameter as string).Contains("Upper"))
            {
                res = true;
            }

            return res;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool res = (bool)value;
            short sign = SignType.None;

            if ((res == true) && (parameter as string).Contains("Lower")
                || (res == false) && (parameter as string).Contains("Upper"))
            {
                sign = SignType.Lower;
            }
            else if ((res == true) && (parameter as string).Contains("Upper")
                || (res == false) && (parameter as string).Contains("Lower"))
            {
                sign = SignType.Upper;
            }

            return sign;
        }
    }
}
