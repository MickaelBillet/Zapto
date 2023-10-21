using Connect.Model;
using System.Threading.Tasks;

namespace Connect.Application
{
	public interface IApplicationConditionServices
	{
		Task<bool?> UpdateCondition(Condition condition);
	}
}
