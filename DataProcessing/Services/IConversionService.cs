namespace DataProcessing.Services
{
    public interface IConversionService
    {
        Task<string> AnalyseAsync(string path);
    }
}