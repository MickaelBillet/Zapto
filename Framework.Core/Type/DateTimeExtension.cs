using System;

namespace Framework.Core.Base
{
    public static class DateTimeExtension
    {
        public static int CompareDay(this DateTime dateTime, DateTime day)
        {
            int res = 0;

            if ((dateTime.Year == day.Year) && (dateTime.Month == day.Month) && (dateTime.Day == day.Day))
            {
                res = 0;
            }
            else if ((dateTime.Year > day.Year)
                    || ((dateTime.Year == day.Year) && (dateTime.Month > day.Month))
                    || ((dateTime.Year == day.Year) && (dateTime.Month == day.Month) && (dateTime.Day > day.Day)))
            {
                res = 1;
            }
            else if ((dateTime.Year < day.Year)
                || ((dateTime.Year == day.Year) || (dateTime.Month < day.Month))
                || ((dateTime.Year == day.Year) && (dateTime.Month == day.Month) && (dateTime.Day < day.Day)))
            {
                res = -1;
            }
            
            return res;
        }
    }
}
