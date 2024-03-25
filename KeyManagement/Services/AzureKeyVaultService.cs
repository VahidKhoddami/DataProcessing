
using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using KeyManagement.Interfaces;

namespace KeyManagement.Services
{
    public class AzureKeyVaultService : ISecretKeyService
    {
        public async Task<string> GetSecret(string secretKey)
        {
            //Endpoint for accessing the key vault
            var kvUri = "https://InternshipKeyValut.vault.azure.net";

            var keyVaultClient = new SecretClient(new Uri(kvUri), new DefaultAzureCredential());
            var secret = await keyVaultClient.GetSecretAsync(secretKey);

            if (secret == null)
                throw new Exception("Secret not found");

            return secret.Value.Value;
        }
    }
}
