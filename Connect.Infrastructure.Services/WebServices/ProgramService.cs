using Connect.Application.Infrastructure;
using Connect.Model;
using Microsoft.Extensions.Configuration;
using Serilog;
using System;
using System.Threading.Tasks;

namespace Connect.Infrastructure.WebServices
{
    public class ProgramService : ConnectWebService, IProgramService
    {
        #region Property

        #endregion

        #region Constructor

        public ProgramService(IServiceProvider serviceProvider, IConfiguration configuration, string httpClientName) : base(serviceProvider, configuration, httpClientName)
        {
        }

        #endregion

        #region Method

        public async Task<Program?> GetProgram(string programId)
        {
            Program? program = null;

            try
            {
                program = await WebService.GetAsync<Program>(ConnectConstants.RestUrlProgramsId, programId, SerializerOptions);
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message);
            }

            return program;
        }

        #endregion
    }
}
