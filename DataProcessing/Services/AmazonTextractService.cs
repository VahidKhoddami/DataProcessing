using Amazon.Textract;
using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using Azure;
using System.IO;
namespace DataProcessing.Services
{
    public class AmazonTextractService : IConversionService
    {
        public async Task<string> AnalyseAsync(string path)
        {
            Amazon.Runtime.BasicAWSCredentials credentials = new Amazon.Runtime.BasicAWSCredentials("AKIASEFQM3Y26UZI6RSD", await GetSecret("AKIASEFQM3Y26UZI6RSD"));
            AmazonTextractClient client = new AmazonTextractClient(credentials, Amazon.RegionEndpoint.CACentral1);

            var memoryStream = new MemoryStream();
            using (Stream stream = new FileStream(path, FileMode.Open))
                stream.CopyTo(memoryStream);

            var result = await client.DetectDocumentTextAsync(new Amazon.Textract.Model.DetectDocumentTextRequest
            {
                Document = new Amazon.Textract.Model.Document
                {
                    Bytes = memoryStream
                }
            });

            return result.Blocks.Where(block => block.BlockType == BlockType.LINE).Select(block => block.Text).Aggregate((a, b) => a + Environment.NewLine + b);
        }

        private async Task<string> GetSecret(string secretKey)
        {
            //Endpoint for accessing key vault
            var kvUri = "https://InternshipKeyValut.vault.azure.net";

            var keyVaultClient = new SecretClient(new Uri(kvUri), new DefaultAzureCredential());

            var secret = await keyVaultClient.GetSecretAsync(secretKey);

            return secret.Value.Value;
        }
    }
}
