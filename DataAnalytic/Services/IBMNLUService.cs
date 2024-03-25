using DataAnalytic.Interfaces;
using IBM.Cloud.SDK.Core.Authentication.Iam;
using IBM.Watson.NaturalLanguageUnderstanding.v1;
using IBM.Watson.NaturalLanguageUnderstanding.v1.Model;
using KeyManagement.Interfaces;

namespace DataAnalytic.Services
{
    public class IBMNLUService : IAnalyticService
    {
        private readonly ISecretKeyService _secretKeyService;
        public IBMNLUService(ISecretKeyService secretKeyService) => _secretKeyService = secretKeyService;

        public async Task<string> AnalyseTextAsync(string text)
        {
            IamAuthenticator authenticator = new IamAuthenticator(
            apikey: await _secretKeyService.GetSecret("IBMNLUKey")
            );

            NaturalLanguageUnderstandingService naturalLanguageUnderstanding = new NaturalLanguageUnderstandingService("2022-04-07", authenticator);
            naturalLanguageUnderstanding.SetServiceUrl(await _secretKeyService.GetSecret("IBMNLUURL"));

            var result = naturalLanguageUnderstanding.Analyze(
                text: text,
                features: new Features()
                {
                    Keywords = new KeywordsOptions()
                    {
                        Sentiment = true,
                        Emotion = true,
                        Limit = 50
                    }
                },
                language: "en"
                );

            return result.Response;

        }
    }
}
