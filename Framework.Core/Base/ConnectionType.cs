namespace Framework.Core.Base
{
	public class ConnectionType
	{
		public string ConnectionString
		{
			get; set;
		}

		public ServerType ServerType
		{
			get; set;
		}

		public OrmType OrmType
		{
			get; set;
		}

        public string GetFileFullName()
        {
            return this.ConnectionString.Split('=')[1].TrimEnd(';');
        }

		public static ServerType GetServerType(string type) => type switch
		{
			"Sqlite" => ServerType.SqlLite,
			"SQLServer" => ServerType.SqlServer,
			"MySql" => ServerType.MySql,
			"PostGreSQL" => ServerType.PostgreSQL,
			_ => ServerType.None,
		};
    }
}
