using System;

namespace Framework.Core.Base
{
    public sealed class TimeSpanText
    {
        private string m_data = null;

        public string Data
        {
            get { return m_data; }
            set { m_data = value; }
        }

        private readonly bool m_isTimeSpan = false;

        public TimeSpanText(string text)
        {
            string[] tmp = text.Split(default(string[]), StringSplitOptions.None);

            TimeSpan timeSpanTmp;

            if (tmp.Length == 2)
            {
                string days = string.Empty;
                string time = string.Empty;

                foreach (char car in tmp[0])
                {

                    if (int.TryParse(new string(car, 1), out int res))
                    {
                        days = days + car;
                    }
                }

                //Test si on récupère bien une durée au format correct
                if (TimeSpan.TryParse(tmp[1] + ".00", out timeSpanTmp))
                {
                    time = tmp[1] + ".00";

                    if (!string.IsNullOrEmpty(days))
                    {
                        days = days + ".";
                    }

                    this.m_data = days + time;

                    this.m_isTimeSpan = true;
                }
                else
                {
                    this.m_data = text;
                }
            }
            else if (tmp.Length == 1)
            {
                //Test si on récupère bien une durée au format correct
                if (TimeSpan.TryParse(tmp[0] + ".00", out timeSpanTmp))
                {
                    this.m_data = tmp[0] + ".00";

                    this.m_isTimeSpan = true;
                }
                else
                {
                    this.m_data = text;
                }
            }
            else
            {
                this.m_data = text;
            }
        }

        public string ConvertFromCulture()
        {
            if (this.m_isTimeSpan)
            {
                return this.m_data.Substring(0, this.m_data.Length - 3);
            }
            else
            {
                return this.m_data;
            }
        }

        public new bool Equals(object obj)
        {
            TimeSpanText objToCompare = obj as TimeSpanText;


            bool res1 = TimeSpan.TryParse(objToCompare.Data, out TimeSpan timeSpanToCompare);

            bool res2 = TimeSpan.TryParse(this.Data, out TimeSpan timeSpan);

            //Les 2 objets sont du type TimeSpan
            if ((res1) && (res2))
            {
                return timeSpan.Equals(timeSpanToCompare);
            }
            //Les 2 objets sont du type String
            else if ((!res1) && (!res2))
            {
                return this.Data.Equals(objToCompare.Data);
            }
            else
            {
                return false;
            }
        }
    }
}
