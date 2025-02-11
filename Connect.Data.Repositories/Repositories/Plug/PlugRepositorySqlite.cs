using Framework.Data.Abstractions;
using System.Diagnostics;

namespace Connect.Data.Repositories
{
    public class PlugRepositorySqlite : PlugRepository
    {
        #region Constructor
        public PlugRepositorySqlite(IDataContextFactory dataContextFactory) : base(dataContextFactory)
        { }
        #endregion

        #region Methods
        public override async Task<int> Upgrade1_1()
        {
            int res = -1;
            try
            {
                await this.DataContextFactory.UseContext(async (context) =>
                {
                    string sql = $"ALTER TABLE Plug " +
                                    $"ADD COLUMN LastCommandDateTime DATETIME";

                    if (context != null)
                    {
                        res = await context.ExecuteNonQueryAsync(sql);
                        if (res == 0)
                        {
                            sql = $"ALTER TABLE Plug " +
                                            $"ADD COLUMN LastCommandSent INTEGER DEFAULT 0";

                            if (context != null)
                            {
                                res = await context.ExecuteNonQueryAsync(sql);
                            }
                        }
                    }
                });
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
