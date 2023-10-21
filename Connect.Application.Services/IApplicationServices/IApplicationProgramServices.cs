using Connect.Model;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace Connect.Application
{
	public interface IApplicationProgramServices
	{
		Task<bool?> DeleteOperationRange(Program program, OperationRange operationRange);
		Task<bool?> DeleteOperationRanges(Program program);
		Task<(bool hasError, bool hasOverLapping)> AddOperationRange(Program program, OperationRange operationRange);
		Task<ObservableCollection<OperationRange>?> GetOperationsRanges(string programId);
	}
}
