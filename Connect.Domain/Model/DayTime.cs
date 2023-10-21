using System;
using System.Collections.Generic;
using Framework.Core.Domain;

namespace Connect.Model
{
    public class DayTime : Item, IEquatable<DayTime>
    {
        #region Property

        public static List<string>? Times { get; set; } = null;

        public static List<DayOfWeek>? Days { get; set; } = null;

        public TimeSpan Time { get; set; }

        public DayOfWeek Day { get; set; }

        #endregion

        #region Method

        public static int Convert(DayOfWeek day)
        {
            int res = 0;

            if (day == DayOfWeek.Monday)
            {
                res = 0;
            }
            else if (day == DayOfWeek.Tuesday)
            {
                res = 1;
            }
            else if (day == DayOfWeek.Wednesday)
            {
                res = 2;
            }
            else if (day == DayOfWeek.Thursday)
            {
                res = 3;
            }
            else if (day == DayOfWeek.Friday)
            {
                res = 4;
            }
            else if (day == DayOfWeek.Saturday)
            {
                res = 5;
            }
            else if (day == DayOfWeek.Sunday)
            {
                res = 6;
            }

            return res;
        }

        /// <summary>
        /// Overloading ==
        /// </summary>
        /// <param name="obj1"></param>
        /// <param name="obj2"></param>
        /// <returns></returns>
        public static bool operator ==(DayTime obj1, DayTime obj2)
        {
            if (ReferenceEquals(obj1, obj2))
            {
                return true;
            }

            if (obj1 is null)
            {
                return false;
            }
            if (obj2 is null)
            {
                return false;
            }

            return (obj1.Day == obj2.Day
                    && obj1.Time == obj2.Time);
        }

        /// <summary>
        /// Overloading Equals
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            if (obj is null)
            {
                return false;
            }
            if (ReferenceEquals(this, obj))
            {
                return true;
            }

            return obj.GetType() == GetType() && Equals((DayTime)obj);
        }

        /// <summary>
        /// Overloading Equals
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public bool Equals(DayTime other)
        {
            if (other is null)
            {
                return false;
            }
            if (ReferenceEquals(this, other))
            {
                return true;
            }

            return this.Day.Equals(other.Day)
                   && this.Time.Equals(other.Time);
        }

        /// <summary>
        /// overloading operator >=
        /// </summary>
        /// <param name="obj1"></param>
        /// <param name="obj2"></param>
        /// <returns></returns>
        public static bool operator >=(DayTime obj1, DayTime obj2)
        {
            return obj1.Day >= obj2.Day
                   && obj1.Time >= obj2.Time;
        }

        /// <summary>
        /// overloading operator <=
        /// </summary>
        /// <param name="obj1"></param>
        /// <param name="obj2"></param>
        /// <returns></returns>
        public static bool operator <=(DayTime obj1, DayTime obj2)
        {
            return obj1.Day <= obj2.Day
                   && obj1.Time <= obj2.Time;
        }

        /// <summary>
        /// Overloading !=
        /// </summary>
        /// <param name="obj1"></param>
        /// <param name="obj2"></param>
        /// <returns></returns>
        public static bool operator !=(DayTime obj1, DayTime obj2)
        {
            if (ReferenceEquals(obj1, obj2))
            {
                return false;
            }

            if (obj1 is null)
            {
                return true;
            }
            if (obj2 is null)
            {
                return true;
            }

            return (obj1.Day != obj2.Day || obj1.Time != obj2.Time);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int hashCode = this.Day.GetHashCode();
                hashCode = (hashCode * 397) ^ this.Time.GetHashCode();
                return hashCode;
            }
        }

        #endregion
    }

    public class DayOfWeekConnect
    {
        private DayOfWeek day;

        #region Property

        public DayOfWeek Day
        {
            get { return day; }

            set { day = value; }
        }

        #endregion
    }

    public class TimeConnect
    {
        private string? time;

        #region Property

        public string? Time
        {
            get { return time; }

            set { time = value; }
        }

        #endregion
    }
}
