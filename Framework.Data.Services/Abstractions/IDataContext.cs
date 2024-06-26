﻿using Framework.Core.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace Framework.Data.Abstractions
{
    public interface IDataContext  : IDisposable
	{
		bool DropDatabase();
		bool CreateDataBase();
		bool DataBaseExists();
		void DetachLocal<T>(T t, string entryId) where T : ItemEntity;
		DbSet<T> Set<T>() where T : class;
		Task<int> ExecuteNonQueryAsync(string sql);
    }
}
