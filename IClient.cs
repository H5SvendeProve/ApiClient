namespace ApiClient;

public interface IClient
{
    Task<string> GetPriceData(string url);
}