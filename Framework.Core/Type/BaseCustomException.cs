using System;

namespace Framework.Core.Base
{
    public class BaseCustomException : Exception
    {
        public int Code { get; }

        public string Description { get; }

        public BaseCustomException(string message, string description, int code) : base(message)
        {
            Code = code;
            Description = description;
        }
    }

    public class CustomErrorResponse
    {
        public int Code { get; set; }
        public string Message { get; set; }
        public string Description { get; set; }
    }
}
