namespace Zapto.Component.Common.Models
{
	public record SensorEventModel : ObjectConnectedModel
	{
        #region Properties
        public int HasLeak { get; set; }
        #endregion
    }
}
