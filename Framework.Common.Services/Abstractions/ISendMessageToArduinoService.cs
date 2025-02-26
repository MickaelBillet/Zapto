﻿using System.Threading.Tasks;

namespace Framework.Common.Services
{
    public interface ISendMessageToArduinoService
    {
        Task<bool> SendMessageAsync(string message);
    }
}
