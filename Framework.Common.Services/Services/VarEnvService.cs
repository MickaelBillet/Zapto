using Framework.Common.Services;
using System;

namespace Framework.Infrastructure.Services
{
    public class VarEnvService : ISecretService
    {
        public string GetSecret(string name)
        {
            string result = null;

            if ((string.IsNullOrEmpty(name) == false) && (Environment.GetEnvironmentVariable(name) != null))
            {
                result = Environment.GetEnvironmentVariable(name);
            }
            else
            {
                throw new ArgumentException(name);
            }
            return result;
        }
    }
}
