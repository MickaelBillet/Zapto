using Connect.Data.Supervisors;
using Connect.Model;
using Framework.Core.Base;

namespace Connect.Test.Data.Supervisor
{
    public class SupervisorLogTest : SupervisorTest
    {
        [Fact]
        public async Task CollectionGetLogsCollection()
        {
            ISupervisor supervisor = new Connect.Data.Supervisors.Supervisor(this.ServiceProvider, this.CreateContext());
            IEnumerable<Logs> logs = await supervisor.GetLogsCollection();

            Assert.NotNull(logs);
            Assert.Collection(logs,
                                item1 => Assert.Equal("0", item1.Id));
        }

        [Fact]
        public async Task GetLogsInf24H()
        {
            ISupervisor supervisor = new Connect.Data.Supervisors.Supervisor(this.ServiceProvider, this.CreateContext());
            IEnumerable<Logs> logs = await supervisor.GetLogsInf24H();

            Assert.NotNull(logs);
        }

        [Fact]
        public async Task AddLog()
        {
            this.ResetDatabase();
            ISupervisor supervisor = new Connect.Data.Supervisors.Supervisor(this.ServiceProvider, this.CreateContext());
            ResultCode code = await supervisor.AddLog(new Logs { Id = "1"});
            IEnumerable<Logs> logs = await supervisor.GetLogsCollection();
            
            Assert.NotNull(logs);
            Assert.True(code == ResultCode.Ok);
            Assert.True(logs.Count() == 2);
        }

        [Fact]
        public async Task LogExists()
        {
            ISupervisor supervisor = new Connect.Data.Supervisors.Supervisor(this.ServiceProvider, this.CreateContext());
            ResultCode code = await supervisor.LogExists("0");

            Assert.True(code == ResultCode.Ok);
        }

        [Fact]
        public async Task GetLogs()
        {
            ISupervisor supervisor = new Connect.Data.Supervisors.Supervisor(this.ServiceProvider, this.CreateContext());
            Logs logs = await supervisor.GetLogs("0");

            Assert.NotNull(logs);
        }

        [Fact]
        public async Task DeleteLogs()
        {
            this.ResetDatabase();
            ISupervisor supervisor = new Connect.Data.Supervisors.Supervisor(this.ServiceProvider, this.CreateContext());
            ResultCode code = await supervisor.DeleteLogs(new Logs { Id = "0" });
            IEnumerable<Logs> logs = await supervisor.GetLogsCollection();

            Assert.NotNull(logs);
            Assert.True(code == ResultCode.Ok);
            Assert.True(logs.Count() == 0);
        }
    }
}
