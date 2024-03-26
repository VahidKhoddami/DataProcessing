using Azure;
using Azure.AI.TextAnalytics;
using DataAnalytic.Interfaces;
using KeyManagement.Interfaces;
using Microsoft.OpenApi.Extensions;
using Newtonsoft.Json;

namespace DataAnalytic.Services
{
    public class AzureTextAnalytics : IAnalyticService
    {
        private readonly ISecretKeyService _secretKeyService;
        public AzureTextAnalytics(ISecretKeyService secretKeyService) => _secretKeyService = secretKeyService;

        public async Task<string> AnalyseTextAsync(string text)
        {
            Uri endpoint = new(await _secretKeyService.GetSecret("AzureSentimentEndpoint"));
            AzureKeyCredential credential = new(await _secretKeyService.GetSecret("AzureSentimentKey"));
            TextAnalyticsClient client = new(endpoint, credential);

            var response = await client.AnalyzeSentimentAsync(text, "en", new AnalyzeSentimentOptions() { IncludeOpinionMining = true });
            var result = "";
            var sentiments = response.Value.Sentences
                .Select(a => a.Opinions
                    .Select(b => new
                    {
                        Keyword = b.Target.Text,
                        Sentiment = b.Target.Sentiment.GetDisplayName(),
                        Score = new
                        {
                            Neutral = b.Target.ConfidenceScores.Neutral,
                            Positive = b.Target.ConfidenceScores.Positive,
                            Negative = b.Target.ConfidenceScores.Negative
                        }
                    }
                ));

            //convert response to json string
            result = JsonConvert.SerializeObject(sentiments);

            return result;
        }
    }
}
