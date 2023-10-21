using System;

namespace Framework.Core.Base
{
    public static class Clock
    {
        private static Func<DateTime> function;

        #region Properties

        public static Func<DateTime> FunctionNow
        {
            set
            {
                function = value ?? (() => DateTime.Now);
            }
        }

        public static DateTime Now
        {
            get
            {
                return function();
            }
        }

        #endregion

        #region Constructor

        static Clock()
        {
            function = () => DateTime.Now;
        }

        #endregion

        #region Methods

        public static void Reset()
        {
            FunctionNow = null;
        }

        #endregion
    }
}
