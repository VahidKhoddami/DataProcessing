namespace DataAnalytic.Interfaces
{
    public interface IAnalyticService
    {
        Task<string> AnalyseTextAsync(string text);
    }
}
