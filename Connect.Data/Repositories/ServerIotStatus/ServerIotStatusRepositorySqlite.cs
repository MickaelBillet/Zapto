using Connect.Data.Session;
using System.Diagnostics;

namespace Connect.Data.Repositories
{
    public class ServerIotStatusRepositorySqlite : ServerIotStatusRepository
    {
        #region Constructor
        public ServerIotStatusRepositorySqlite(IDalSession session) : base(session)
        { }
        #endregion

        #region Methods
        public override async Task<int> CreateTable()
        {
            int res = -1;
            try
            {
                string sql = $"CREATE TABLE ServerIotStatus " +
                                $"(ConnectionDate DATETIME, " +
                                $"IpAddress BLOB, " +
                                $"Id BLOB, " +
                                $"CreationDateTime DATETIME);";

                if (this.DataContext != null)
                {
                    res = await this.DataContext.ExecuteNonQueryAsync(sql);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
            return res;
        }
        #endregion
    }
}
