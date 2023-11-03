using System;
using Framework.Core.Domain;


namespace Connect.Model
{
    public class OperationRange : Item
    {
        private Condition? condition = null;

        #region Property

        public string? ProgramId
        {
            get; set;
        }

        public string? ConditionId 
		{
            get; set;
		}

        public Condition? Condition
        {
            get { return condition; }

            set { SetProperty<Condition?>(ref condition, value); }
        }

        public TimeSpan StartTime { get; set; }

        public TimeSpan EndTime { get; set; }

        public DayOfWeek Day { get; set; }

        public Boolean AllDay { get; set; }

        #endregion

        #region Constructor

        public OperationRange(): base() 
		{
		}

        #endregion

        #region Method

        #endregion
    }
}
