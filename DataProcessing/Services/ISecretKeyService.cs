namespace DataProcessing.Services
{
    public interface ISecretKeyService
    {
        Task<string> GetSecret(string secretKey);
    }
}
