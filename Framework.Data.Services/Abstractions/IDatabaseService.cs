using System;
using System.Threading.Tasks;

namespace Framework.Data.Abstractions
{
    public interface IDatabaseService : IDisposable
	{
		Task ConfigureDatabase();
    }
}
