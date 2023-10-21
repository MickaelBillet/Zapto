namespace AirZapto.Model
{
    public static class Mode
    {
        public const short Manual = 0;
        public const short Programing = 1;
    }

    public static class SensorStatus
	{
        public static int None = 0;
        public static int Good = 1;
        public static int Average = 2;
        public static int Bad = 3;
    }

    public static class SensorMode
    {
        public static int Measure = 0;
        public static int Calibration = 1;
        public static int Restart = 2;
        public static int Initial = 3;
        public static int Startup = 4;
        public static int Preheating = 5;
        public static int Data = 99;
    }

    public static class Status
    {
        public const short Off = 0;
        public const short On = 1;
    }

    public static class Command
	{
        public static int Connection = 0;
        public static int Calibration = 1;
        public static int Restart = 2;
	}

    public static class DeviceType
    {
        public const int None = 0;
        public const int Module = 1;
        public const int Outlet = 2;
        public const int Sensor_Temperature = 4;
        public const int Sensor_Humidity = 8;
        public const int Sensor_Pressure = 16;
        public const int Sensor_CO2 = 32;
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
}
