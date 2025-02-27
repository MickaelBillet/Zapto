using Framework.Data.Abstractions;
using Serilog;

namespace Connect.Data.Repositories
{
    public class ServerIotStatusRepositorySqlite : ServerIotStatusRepository
    {
        #region Constructor
        public ServerIotStatusRepositorySqlite(IDataContextFactory dataContextFactory) : base(dataContextFactory)
        { }
        #endregion

        #region Methods
        public override async Task<int> CreateTableServerIotStatus()
        {
            int res = -1;
            try
            {
                await this.DataContextFactory.UseContext(async (context) =>
                {
                    string sql = $"CREATE TABLE ServerIotStatus " +
                                $"(ConnectionDate DATETIME, " +
                                $"IpAddress BLOB, " +
                                $"Id BLOB, " +
                                $"CreationDateTime DATETIME);";

                    if (context != null)
                    {
                        res = await context.ExecuteNonQueryAsync(sql);
                    }
                });
            }
            catch (Exception ex)
            {
                Log.Error("CreateTableServerIotStatus : " + ex.Message);
            }
            return res;
        }
        #endregion
    }
}
