using DataAnalytic.Interfaces;
using KeyManagement.Interfaces;

namespace DataAnalytic.Services
{
    public class ProviderFactory
    {
        private readonly ISecretKeyService _secretKeyService;
        public ProviderFactory(ISecretKeyService secretKeyService) => _secretKeyService = secretKeyService;
        public IAnalyticService GetTextService(string provider)
        {
            switch (provider.ToLower())
            {
                case "ibm":
                    return new IBMNLUService(_secretKeyService);
                case "azure":
                    return new AzureTextAnalytics(_secretKeyService);
                case "amazon":
                    return new AWSComprehendService(_secretKeyService);
                default:
                    throw new ArgumentOutOfRangeException(provider, "The provider name not found!");
            }
        }
    }
}
