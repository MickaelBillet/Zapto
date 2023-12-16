using Framework.Data.Abstractions;
using System.Diagnostics;

namespace Connect.Data.Repositories
{
    public class PlugRepositorySqlite : PlugRepository
    {
        #region Constructor
        public PlugRepositorySqlite(IDalSession session) : base(session)
        { }
        #endregion

        #region Methods
        public override async Task<int> Upgrade1_1()
        {
            int res = -1;
            try
            {
                string sql = $"ALTER TABLE Plug " +
                                $"ADD COLUMN LastCommandDateTime DATETIME";

                if (this.DataContext != null)
                {
                    res = await this.DataContext.ExecuteNonQueryAsync(sql);
                }

                if (res == 0)
                {
                    sql = $"ALTER TABLE Plug " +
                                    $"ADD COLUMN LastCommandSent INTEGER DEFAULT 0";

                    if (this.DataContext != null)
                    {
                        res = await this.DataContext.ExecuteNonQueryAsync(sql);
                    }
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
