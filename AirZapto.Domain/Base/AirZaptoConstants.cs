namespace AirZapto
{
	public static class AirZaptoConstants
	{
		public static readonly string RestUrlSensors = @"sensors/";
		public static readonly string RestUrlSensorCalibration = @"sensors/{0}/calibration/";
		public static readonly string RestUrlSensorRestart = @"sensors/{0}/restart/";
		public static readonly string RestUrlSensorData = @"sensordata/{0}/duration/{1}";
		public static readonly string RestUrlSensor = @"sensors/{0}";
		public static readonly string RestUrlHealthCheck = @"health";

        public static readonly string Application_Prefix = @"/AirZapto";

        public const string ConnectionStringAirZaptotKey = "ConnectionStringAirZapto";
        public const string ServerTypeAirZaptoKey = "ServerTypeAirZapto";
    }
}
