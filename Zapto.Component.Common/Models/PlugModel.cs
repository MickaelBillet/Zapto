using Connect.Model;

namespace Zapto.Component.Common.Models
{
	public sealed record PlugModel : ObjectConnectedModel
	{
        #region Properties
        public string? FileNameTypeImage
        {
            get
            {
                if ((this.ConditionType & ParameterType.Humidity) == (ParameterType.Humidity))
                {
                    return @"Images\icons8fan40.png";

                }
                else if ((this.ConditionType & ParameterType.Temperature) == (ParameterType.Temperature))
                {
                    return @"Images\icons8radiator50.png"; ;
                }
                else
                {
                    return null;
                }
            }
        }

        public string? FileNameStatusImage
        {
            get
            {
                if (this.Status == Connect.Model.Status.ON)
                {
                    return @"Images\LightningYellow.png";
                }
                else if (this.Status == Connect.Model.Status.OffON)
                {
                    return @"Images\LightningBlue.png";
                }
                else if (this.Status == Connect.Model.Status.OnOFF)
                {
                    return @"Images\LightningBlueEmpty.png";
                }
                else
                {
                    return null;
                }
            }
        }
        public int Status { get; set; } = Connect.Model.Status.OFF;
        public int? ConditionType { get; set; } = ParameterType.None;
        public double WorkingDuration { get; set; }
        public int Command { get; set; } = CommandType.Off;
        #endregion
    }
}
