namespace DataProcessing.Services
{
    public static class ProviderFactory
    {
        public static IConversionService GetImageService(string provider)
        {
            switch (provider.ToLower())
            {
                case "google":
                    throw new NotImplementedException();
                case "azure":
                    return new AzureVisionService();
                case "amazon":
                    throw new NotImplementedException();
                default:
                    throw new ArgumentOutOfRangeException(provider, "The provider name not found!");
            }
        }
    }
}
