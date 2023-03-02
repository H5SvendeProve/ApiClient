public class Client : IClient
{
    public Client()
    {
        
    }

    public async Task<string> GetPriceData(string url)
    {
        using var client = new HttpClient();

        var response = await client.GetAsync(url);

        if (response.IsSuccessStatusCode)
        {
            var content = await response.Content.ReadAsStringAsync();

            return content;
        }
        return string.Empty;
    }
}
