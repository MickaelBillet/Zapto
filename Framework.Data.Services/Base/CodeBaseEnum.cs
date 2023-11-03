namespace Framework.Core.Data
{
	public enum CodeDatabaseEnum
	{
		e_None = 0,
		e_Ok = 1,
		e_OpenConnectionError = 2,
		e_CreateDatabaseError = 3,
		e_InitializeError = 4,
		e_DropDatabaseError = 5,
		e_AlreadyExist = 6,
		e_Unauthorized = 7,
	}
}
