namespace Framework.Core.Base
{
    public enum ResultCode
    {
        Ok = 0,
        ArgumentRequired = 1,
        ItemAlreadyExist = 2,
        ItemNotFound = 3,
        CouldNotCreateItem = 4,
        CouldNotUpdateItem = 5,
        CouldNotDeleteItem = 6,
    }

    public enum ServerType
    {
        None = 0,
        SqlLite = 1,
        MySql = 2,
        SqlServer = 4,
        PostgreSQL = 5,
    }

    public enum OrmType
	{
        None = 0,
        EntityFramework = 1,
        Dapper = 2,
        Ado = 3,
	}

}
