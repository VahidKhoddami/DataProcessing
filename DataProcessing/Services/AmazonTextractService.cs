using Amazon.Textract;
using KeyManagement.Interfaces;
namespace DataProcessing.Services
{
    public class AmazonTextractService : IConversionService
    {
        private readonly ISecretKeyService _secretKeyService;
        public AmazonTextractService(ISecretKeyService secretKeyService) => _secretKeyService = secretKeyService;
        public async Task<string> AnalyseAsync(string path)
        {
            Amazon.Runtime.BasicAWSCredentials credentials = new Amazon.Runtime.BasicAWSCredentials("AKIASEFQM3Y26UZI6RSD", await _secretKeyService.GetSecret("AKIASEFQM3Y26UZI6RSD"));
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

    }
}
