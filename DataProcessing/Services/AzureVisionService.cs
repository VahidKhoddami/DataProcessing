using Azure;
using Azure.AI.Vision.ImageAnalysis;
using KeyManagement.Interfaces;
using System.Text;

namespace DataProcessing.Services
{
    public class AzureVisionService : IConversionService
    {
        private readonly ISecretKeyService _secretKeyService;
        public AzureVisionService(ISecretKeyService secretKeyService) => _secretKeyService = secretKeyService;
        public async Task<string> AnalyseAsync(string path)
        {

            ImageAnalysisClient client = new ImageAnalysisClient(
                                              new Uri(await _secretKeyService.GetSecret("CognitiveServicesEndpoint")),
                                              new AzureKeyCredential(await _secretKeyService.GetSecret("CognitiveServicesKey")));

            using FileStream stream = new FileStream(path, FileMode.Open);
            var result = await client.AnalyzeAsync(BinaryData.FromStream(stream), VisualFeatures.Read);

            var resultStr = new StringBuilder();
            foreach (var line in result.Value.Read.Blocks.SelectMany(block => block.Lines))
                resultStr.AppendLine(line.Text);

            return resultStr.ToString();
        }

    }
}
