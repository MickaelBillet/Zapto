namespace Framework.Infrastructure.Services
{
    public interface IEncrypt
    {
		string EncryptString(string plainText, string passPhrase);
		string DecryptString(string cipherText, string passPhrase);
        string GetMd5Hash(string input);
        bool VerifyMd5Hash(string input, string hash);
    }
}
