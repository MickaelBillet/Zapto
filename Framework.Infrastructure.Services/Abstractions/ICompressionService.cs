using System.Threading.Tasks;

namespace Framework.Infrastructure.Services
{
    public interface ICompressionService
	{
		Task<byte[]> CompressData(byte[] data);

		Task<byte[]> DeCompressData(byte[] data);
	}
}
