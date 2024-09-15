namespace Connect.Model
{
    public static class ConnectConstants
    {
        public static readonly string PROTOCOL_TYPE = "NRS";
        public static readonly int PortPlugCommand = 5001;
        public static readonly int PortPlugStatus = 5002;
        public static readonly int PortSensorData = 5003;
        public static readonly int PortConnectionData = 5004;
		public static readonly int PortServerIotStatus = 5005;
		public static readonly int PortSensorEvent = 5007;

		//URL of REST service
		public const string RestUrlLocations = @"connect/locations/";
		public const string RestUrlLocationsId = @"connect/locations/{0}/";
		public const string RestUrlLocationRooms = @"connect/locations/{0}/rooms/";
        public const string RestUrlLocationTest = @"connect/locations/test";
        public const string RestUrlRoomsId = @"connect/rooms/{0}/";
		public const string RestUrlPlugCommand = @"connect/plugs/{0}/mode/";
		public const string RestUrlPlugOrder = @"connect/plugs/{0}/onoff/";
		public const string RestUrlPlugsfilter = @"connect/plugs?address={0}&unit={1}";
		public const string RestUrlProgramsId = @"connect/programs/{0}/";
		public const string RestUrlConnectedObjectId = @"connect/connectedobjects/{0}/";
        public const string RestUrlProgramOperationRanges = @"connect/programs/{0}/operationranges/";
        public const string RestUrlOperationRanges = @"connect/operationranges/";
		public const string RestUrlOperationRangesId = @"connect/operationranges/{0}/";
		public const string RestUrlConditions = @"connect/conditions/";
		public const string RestUrlConditionsId = @"connect/conditions/{0}/";
		public const string RestUrlNotifications = @"connect/notifications/";
		public const string RestUrlNotificationsId = @"connect/notifications/{0}/";
        public const string RestUrlConnectedObjectNotifications = @"connect/connectedobjects/{0}/notifications/";
        public const string RestUrlRoomNotifications = @"connect/rooms/{0}/notifications/";
        public const string RestUrlHealthCheck = @"connect/health";
		public const string RestUrlSensorNoLeak = @"connect/sensors/{0}/noleak/";
        public const string RestUrlClientApps = @"connect/clientapps/";
        public const string RestUrlClientAppsToken = @"connect/clientapps/token/{0}/";
		public const string RestUrlRoomOperatingData = @"connect/data/date/{0}/rooms/{1}/";
		public const string RestUrlMaxDateOperatingData = @"connect/data/rooms/{0}/datemax/";
        public const string RestUrlMinDateOperatingData = @"connect/data/rooms/{0}/datemin/";
        public const string RestUrlLogs = @"connect/logs/";

        public const int ValidityPeriodLoginDay = 7;
        public const int MinutesPerDay = 1440;
        public const int DayPerWeek = 7;

		public const string ArduinoServer = "192.168.1.145";
        public const string BMESensor1 = "192.168.1.42";
        public const string BMESensor2 = "192.168.1.46";
        public const string BMESensor3 = "192.168.1.3";

		public const string Type_F007th = "F007TH";
		public const string Type_HTUD21DF = "HTUD21DF";
		public const string Type_MC22_1527 = "MC22_1527";

        public static readonly string Application_Prefix = @"/Connect";
    }
}
