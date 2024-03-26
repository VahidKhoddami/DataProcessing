
using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using KeyManagement.Interfaces;

namespace KeyManagement.Services
{
    public class AzureKeyVaultService : ISecretKeyService
    {
        //create a json object to store the secret in memory as a singltone
        private static readonly Dictionary<string, string> _secrets = new();

        public async Task<string> GetSecret(string secretKey)
        {
            //check if the secret is already in memory
            if (_secrets.ContainsKey(secretKey))
                return _secrets[secretKey];

            //Endpoint for accessing the key vault
            var kvUri = "https://InternshipKeyValut.vault.azure.net";

            var keyVaultClient = new SecretClient(new Uri(kvUri), new DefaultAzureCredential());

            var secret = await keyVaultClient.GetSecretAsync(secretKey);

            if (secret == null)
                throw new Exception("Secret not found");

            //store the secret in memory
            _secrets.Add(secretKey, secret.Value.Value);

            return secret.Value.Value;
        }
    }
}
