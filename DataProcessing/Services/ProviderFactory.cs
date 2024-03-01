namespace DataProcessing.Services
{
    public static class ProviderFactory
    {
        public static IConversionService GetImageService(string provider)
        {
            switch (provider.ToLower())
            {
                case "azure":
                    return new AzureVisionService();
                case "amazon":
                    return new AmazonTextractService();
                default:
                    throw new ArgumentOutOfRangeException(provider, "The provider name not found!");
            }
        }
    }
}
