using Framework.Core.Base;
using Framework.Core.Domain;
using Microsoft.Extensions.DependencyInjection;
using Serilog.Events;

namespace Connect.Data.Supervisors.Tests
{
    public class SupervisorLogTest : SupervisorBase
    {
        [Fact]
        public async Task AddLog()
        {
            //Arrange
            this.CreateHost();
            await this.Initialyse();

            //Act
            ISupervisorLog supervisor = this.HostApplication!.Services.GetRequiredService<ISupervisorLog>();
            ResultCode code = await supervisor.AddLog(new Logs()
            {
                Date = Clock.Now.ToUniversalTime(),
                Level = "Debug",
                Exception = null,
                RenderedMessage = "Error",
            });

            //Assert
            Assert.True(code == ResultCode.Ok);
        }
    }
}
