﻿using Connect.Model;
using Framework.Core.Base;

namespace Connect.Data
{
    public interface ISupervisorSensor : ISupervisor
    {
        Task<IEnumerable<Sensor>> GetSensors();
        Task<Sensor> GetSensor(string id);
        Task<Sensor> GetSensor(string? type, string? channel);
        Task<ResultCode> AddSensor(Sensor sensor);
        Task<(ResultCode, Sensor)> UpdateSensor(Sensor sensor);
    }
}
