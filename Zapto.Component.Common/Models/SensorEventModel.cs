namespace Zapto.Component.Common.Models
{
	public sealed record SensorEventModel : ObjectConnectedModel
	{
        #region Properties
        public int HasLeak { get; set; }
        #endregion
    }
}
