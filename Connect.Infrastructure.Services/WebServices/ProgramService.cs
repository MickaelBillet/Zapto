using Connect.Application.Infrastructure;
using Connect.Model;
using Microsoft.Extensions.Configuration;
using System;
using System.Threading.Tasks;

namespace Connect.Infrastructure.WebServices
{
    public class ProgramService : ConnectWebService, IProgramService
    {
        #region Property

        #endregion

        #region Constructor

        public ProgramService(IServiceProvider serviceProvider, string httpClientName) : base(serviceProvider, httpClientName)
        {
        }

        #endregion

        #region Method

        public async Task<Program?> GetProgram(string programId)
        {
            return await WebService.GetAsync<Program>(ConnectConstants.RestUrlProgramsId, programId, SerializerOptions); ;
        }

        #endregion
    }
}
