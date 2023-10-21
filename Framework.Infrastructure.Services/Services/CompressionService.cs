using System.IO;
using System.IO.Compression;
using System.Threading.Tasks;

namespace Framework.Infrastructure.Services
{
    internal class CompressionService : ICompressionService
	{
		public async Task<byte[]> CompressData(byte[] data)
		{
			using (MemoryStream outputStream = new MemoryStream())
			{
				using (DeflateStream stream = new DeflateStream(outputStream, CompressionMode.Compress))
				{
					await stream.WriteAsync(data, 0, data.Length);
				}

				return outputStream.ToArray();
			}
		}

		public async Task<byte[]> DeCompressData(byte[] data)
		{
			using (MemoryStream decompressedStream = new MemoryStream())
			{
				using (MemoryStream compressStream = new MemoryStream(data))
				{
					using (DeflateStream stream = new DeflateStream(compressStream, CompressionMode.Decompress))
					{
						await stream.CopyToAsync(decompressedStream);
					}
				}

				return decompressedStream.ToArray();
			}
		}
	}
}
