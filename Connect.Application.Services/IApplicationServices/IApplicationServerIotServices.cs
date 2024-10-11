﻿using Framework.Core.Model;
using System.Threading.Tasks;

namespace Connect.Application
{
    public interface IApplicationServerIotServices
	{
        Task ReadStatus(SystemStatus? status);
    }
}
