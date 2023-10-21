namespace Connect.Model
{
    public static class Mode
    {
        public const short Manual = 0;
        public const short Programing = 1;
    }

    public static class Order
	{
        public const int Off = 0;
        public const int On = 1;
	}

    public static class CommandType
	{
        public const int Off = 1;
        public const int Manual = 2;
        public const int Programing = 3;
	}

    public static class Status
    {
        public const short OFF = 0;
        public const short ON = 1;
        public const short OnOFF = 2;
        public const short OffON = 3;
    }

    public static class DeviceType
    {
        public const int None = 0;
        public const int Module = 1;
        public const int Outlet = 2;
        public const int Sensor_Temperature = 4;
        public const int Sensor_Humidity = 8;
        public const int Sensor_Pressure = 16;
        public const int Sensor_Water_Leak = 32;
    }

    public static class ErrorType
    {
        public const short None = 0;
        public const short ErrorSoftware = 1;
        public const short ErrorWebService = 2;
        public const short Warning = 3;
    }

    public static class ParameterType
    {
        public const short None = 0;
        public const short Temperature = 1;
        public const short Humidity = 2;
    }

    public static class SignType
    {
        public const short None = 0;
        public const short Equal = 1;
        public const short Lower = 2;
        public const short Upper = 3;
    }

    public static class RoomType
	{
        public const short None = 0;
        public const short SmallBedroom = 1;
        public const short Bedroom = 2;
        public const short Bathroom = 3;
        public const short Kitchen = 4;
	}

    public static class ServiceAlertType
    {
        public const short None = 0;
        public const short SignalR = 1;
        public const short Firebase = 2;
    }
}
