using Amazon.Comprehend;
using Amazon.Comprehend.Model;
using Amazon.Runtime;
using DataAnalytic.Interfaces;
using KeyManagement.Interfaces;
using Newtonsoft.Json;

namespace DataAnalytic.Services
{
    public class AWSComprehendService : IAnalyticService
    {
        private readonly ISecretKeyService _secretKeyService;
        public AWSComprehendService(ISecretKeyService secretKeyService) => _secretKeyService = secretKeyService;
        public async Task<string> AnalyseTextAsync(string text)
        {
            var credentials = new BasicAWSCredentials("AKIASEFQM3Y26UZI6RSD", await _secretKeyService.GetSecret("AKIASEFQM3Y26UZI6RSD"));
            var client = new AmazonComprehendClient(credentials, Amazon.RegionEndpoint.CACentral1);
            var request = new DetectTargetedSentimentRequest
            {
                Text = text,
                LanguageCode = "en"
            };
            var response = await client.DetectTargetedSentimentAsync(request);
            var result = response.Entities
                        .SelectMany(q => q.Mentions
                        .Select(a =>
                            new
                            {
                                Keyword = a.Text,
                                Sentiment = a.MentionSentiment.Sentiment.Value,
                                Score = a.Score
                            }));
            return JsonConvert.SerializeObject(result);
        }
    }
}
