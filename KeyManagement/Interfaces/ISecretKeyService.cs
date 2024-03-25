namespace KeyManagement.Interfaces
{
    public interface ISecretKeyService
    {
        Task<string> GetSecret(string secretKey);
    }
}
