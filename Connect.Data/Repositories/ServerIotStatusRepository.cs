using Connect.Data.Entities;
using Connect.Data.Services.Repositories;
using Framework.Data.Abstractions;
using System.Data.SqlClient;
using System.Diagnostics;

namespace Connect.Data.Repositories
{
    public class ServerIotStatusRepository : Repository<ServerIotStatusEntity>, IServerIotStatusRepository
    {
        #region Constructor
        public ServerIotStatusRepository(IDataContext dataContext) : base(dataContext)
        { }
        #endregion

        #region Methods
        public async Task<int> CreateTable()
        {
            int res = 0;
            try
            {
                string? connectionString = this.DataContext?.Connection?.ConnectionString;
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string sql = $"CREATE TABLE ServerIotStatus " +
                                    $"ADD COLUMN ConnectionDate DATETIME, " +
                                    $"ADD COLUMN IpAddress varbinary(16), " +
                                    $"ADD COLUMN Id DATETIME, varbinary(64)" +
                                    $"ADD COLUMN CreationDateTime DATETIME";

                    SqlCommand sqlQuery = new SqlCommand(sql, new SqlConnection());
                    res = await sqlQuery.ExecuteNonQueryAsync();
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
