using Azure;
using Azure.AI.Vision.ImageAnalysis;
using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using System.Text;

namespace DataProcessing.Services
{
    public class AzureVisionService : IConversionService
    {
        public async Task<string> AnalyseAsync(string path)
        {
            (var key,var endpoint)=await GetKeyAndEndpoint();

            ImageAnalysisClient client = new ImageAnalysisClient(
                                              new Uri(endpoint.Value.Value),
                                              new AzureKeyCredential(key.Value.Value));

            using FileStream stream = new FileStream(path, FileMode.Open);
            var result = await client.AnalyzeAsync(BinaryData.FromStream(stream), VisualFeatures.Read);
            
            var resultStr=new StringBuilder();
            foreach (var line in result.Value.Read.Blocks.SelectMany(block => block.Lines))
                resultStr.AppendLine(line.Text);

            return resultStr.ToString();
        }

        private async Task<Tuple<Response<KeyVaultSecret>, Response<KeyVaultSecret>>> GetKeyAndEndpoint()
        {
            //variables for retrieving the key and endpoint from your key vault.
            //Set these variables to the names you created for your secrets
            const string keySecretName = "CognitiveServicesKey";
            const string endpointSecretName = "CognitiveServicesEndpoint";

            //Endpoint for accessing your key vault
            var kvUri = "https://InternshipKeyValut.vault.azure.net";

            var keyVaultClient = new SecretClient(new Uri(kvUri), new DefaultAzureCredential());

            //Key and endpoint secrets retrieved from your key vault
            var key = await keyVaultClient.GetSecretAsync(keySecretName);
            var endpoint = await keyVaultClient.GetSecretAsync(endpointSecretName);

            return Tuple.Create(key, endpoint);
        }
    }
}
