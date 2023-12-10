using System;

namespace Framework.Core.Base
{
    public class DurationConverter
    {
        public static object Convert(object value)
        {
            if (value is uint)
            {
                uint duration = (uint)value;
                TimeSpan timeSpan;

                timeSpan = new TimeSpan(duration * 60 * 10000000L);

                if (timeSpan.Days > 0)
                {
                    return timeSpan.Days.ToString() + "." + timeSpan.Hours.ToString("D2") + ":" + timeSpan.Minutes.ToString("D2") + ":" + timeSpan.Seconds.ToString("D2");
                }
                else
                {
                    return timeSpan.Hours.ToString("D2") + ":" + timeSpan.Minutes.ToString("D2") + ":" + timeSpan.Seconds.ToString("D2");
                }
            }
            else if (value is string)
            {
                char[] charsToTrim = { ' ' };

                string tmp = (value as string).TrimEnd(charsToTrim);

                return new TimeSpanText(tmp).ConvertFromCulture();
            }

            return string.Empty;
        }
    }
}
