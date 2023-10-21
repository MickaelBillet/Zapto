namespace Framework.Core.Base
{
    public static class ErrorType
    {
        public const short None = 0;
        public const short ErrorSoftware = 1;
        public const short ErrorWebService = 2;
        public const short Warning = 3;
    }

    public static class AnswerType
    {
        public const short No = 0;
        public const short Yes = 1;
        public const short Later = 2;
    }

    public static class RunningStatus
    {
        public const byte None = 0;
        public const byte UnHealthy = 1;
        public const byte Healthy = 2;
        public const byte Degraded = 3;
    }
}
