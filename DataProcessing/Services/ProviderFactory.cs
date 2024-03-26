using KeyManagement.Interfaces;

namespace DataProcessing.Services
{
    public class ProviderFactory
    {
        private readonly ISecretKeyService _secretKeyService;
        public ProviderFactory(ISecretKeyService secretKeyService) => _secretKeyService = secretKeyService;

        public IConversionService GetImageService(string provider)
        {
            switch (provider.ToLower())
            {
                case "azure":
                    return new AzureVisionService(_secretKeyService);
                case "amazon":
                    return new AmazonTextractService(_secretKeyService);
                default:
                    throw new ArgumentOutOfRangeException(provider, "The provider name not found!");
            }
        }
        public IConversionService GetAudioService(string provider)
        {
            switch (provider.ToLower())
            {
                case "assemblyai":
                    return new AssemblyAIAudioService(_secretKeyService);
                case "azure":
                    return new AzureSpeechService(_secretKeyService);
                default:
                    throw new ArgumentOutOfRangeException(provider, "The provider name not found!");
            }
        }
    }
}
