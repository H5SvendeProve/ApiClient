using System.Text.Json;

namespace ApiClient;

public class ApiHandler
{

    //https://www.elprisenligenu.dk/api/v1/prices/2023/02-27_DK2.json

    private const string WestDenmark = "_DK1.json";

    private const string EastDenmark = "_DK2.json";

    private readonly IClient client;

    private static readonly Dictionary<string, string> AllConfigs = MyConfig.GetAllAppSettings();

    public ApiHandler()
    {
        client = new Client();
    }

    private static string BuildUrl(string url, Locations location)
    {
        var dateNow = DateTime.Now;

        var year = dateNow.ToString("yyyy");

        var monthDay = dateNow.ToString("MM") + "-" + dateNow.ToString("dd");

        if (!url.EndsWith("/"))
        {
            url = url + "/";
        }

        url = url + year + "/" + monthDay;

        switch (location)
        {
            case Locations.WestDenmark:
                url = url + WestDenmark;
                break;
            case Locations.EastDenmark:
                url = url + EastDenmark;
                break;
        }

        return url;
    }

    private string GetPricesInDenmark(Locations location)
    {
        var url = AllConfigs["elprisenLigeNuUrl"];

        if (string.IsNullOrEmpty(url))
        {
            throw new ArgumentException("url is missing in app.cofig key : elprisenLigeNuUrl");
        }

        url = BuildUrl(url, location);

        var val = client.GetPriceData(url);

        return val.Result;
    }

    private List<ElectricityPriceDTO> MakePricesFromJson(string electricityPricesJson, Locations location)
    {
        // test json
        //string json = "[{\"DKK_per_kWh\":0.99731,\"EUR_per_kWh\":0.134,\"EXR\":7.442628,\"time_start\":\"2023-02-28T00:00:00+01:00\",\"time_end\":\"2023-02-28T01:00:00+01:00\"},{\"DKK_per_kWh\":0.92899,\"EUR_per_kWh\":0.12482,\"EXR\":7.442628,\"time_start\":\"2023-02-28T01:00:00+01:00\",\"time_end\":\"2023-02-28T02:00:00+01:00\"},{\"DKK_per_kWh\":0.92162,\"EUR_per_kWh\":0.12383,\"EXR\":7.442628,\"time_start\":\"2023-02-28T02:00:00+01:00\",\"time_end\":\"2023-02-28T03:00:00+01:00\"},{\"DKK_per_kWh\":0.92728,\"EUR_per_kWh\":0.12459,\"EXR\":7.442628,\"time_start\":\"2023-02-28T03:00:00+01:00\",\"time_end\":\"2023-02-28T04:00:00+01:00\"},{\"DKK_per_kWh\":0.93963,\"EUR_per_kWh\":0.12625,\"EXR\":7.442628,\"time_start\":\"2023-02-28T04:00:00+01:00\",\"time_end\":\"2023-02-28T05:00:00+01:00\"},{\"DKK_per_kWh\":1.05246,\"EUR_per_kWh\":0.14141,\"EXR\":7.442628,\"time_start\":\"2023-02-28T05:00:00+01:00\",\"time_end\":\"2023-02-28T06:00:00+01:00\"},{\"DKK_per_kWh\":1.2092,\"EUR_per_kWh\":0.16247,\"EXR\":7.442628,\"time_start\":\"2023-02-28T06:00:00+01:00\",\"time_end\":\"2023-02-28T07:00:00+01:00\"},{\"DKK_per_kWh\":1.32471,\"EUR_per_kWh\":0.17799,\"EXR\":7.442628,\"time_start\":\"2023-02-28T07:00:00+01:00\",\"time_end\":\"2023-02-28T08:00:00+01:00\"},{\"DKK_per_kWh\":1.38463,\"EUR_per_kWh\":0.18604,\"EXR\":7.442628,\"time_start\":\"2023-02-28T08:00:00+01:00\",\"time_end\":\"2023-02-28T09:00:00+01:00\"},{\"DKK_per_kWh\":1.19313,\"EUR_per_kWh\":0.16031,\"EXR\":7.442628,\"time_start\":\"2023-02-28T09:00:00+01:00\",\"time_end\":\"2023-02-28T10:00:00+01:00\"},{\"DKK_per_kWh\":0.97037,\"EUR_per_kWh\":0.13038,\"EXR\":7.442628,\"time_start\":\"2023-02-28T10:00:00+01:00\",\"time_end\":\"2023-02-28T11:00:00+01:00\"},{\"DKK_per_kWh\":0.90212,\"EUR_per_kWh\":0.12121,\"EXR\":7.442628,\"time_start\":\"2023-02-28T11:00:00+01:00\",\"time_end\":\"2023-02-28T12:00:00+01:00\"},{\"DKK_per_kWh\":0.86863,\"EUR_per_kWh\":0.11671,\"EXR\":7.442628,\"time_start\":\"2023-02-28T12:00:00+01:00\",\"time_end\":\"2023-02-28T13:00:00+01:00\"},{\"DKK_per_kWh\":0.8658,\"EUR_per_kWh\":0.11633,\"EXR\":7.442628,\"time_start\":\"2023-02-28T13:00:00+01:00\",\"time_end\":\"2023-02-28T14:00:00+01:00\"},{\"DKK_per_kWh\":0.86893,\"EUR_per_kWh\":0.11675,\"EXR\":7.442628,\"time_start\":\"2023-02-28T14:00:00+01:00\",\"time_end\":\"2023-02-28T15:00:00+01:00\"},{\"DKK_per_kWh\":0.96754,\"EUR_per_kWh\":0.13,\"EXR\":7.442628,\"time_start\":\"2023-02-28T15:00:00+01:00\",\"time_end\":\"2023-02-28T16:00:00+01:00\"},{\"DKK_per_kWh\":1.12168,\"EUR_per_kWh\":0.15071,\"EXR\":7.442628,\"time_start\":\"2023-02-28T16:00:00+01:00\",\"time_end\":\"2023-02-28T17:00:00+01:00\"},{\"DKK_per_kWh\":1.28854,\"EUR_per_kWh\":0.17313,\"EXR\":7.442628,\"time_start\":\"2023-02-28T17:00:00+01:00\",\"time_end\":\"2023-02-28T18:00:00+01:00\"},{\"DKK_per_kWh\":1.4429,\"EUR_per_kWh\":0.19387,\"EXR\":7.442628,\"time_start\":\"2023-02-28T18:00:00+01:00\",\"time_end\":\"2023-02-28T19:00:00+01:00\"},{\"DKK_per_kWh\":1.44223,\"EUR_per_kWh\":0.19378,\"EXR\":7.442628,\"time_start\":\"2023-02-28T19:00:00+01:00\",\"time_end\":\"2023-02-28T20:00:00+01:00\"},{\"DKK_per_kWh\":1.30164,\"EUR_per_kWh\":0.17489,\"EXR\":7.442628,\"time_start\":\"2023-02-28T20:00:00+01:00\",\"time_end\":\"2023-02-28T21:00:00+01:00\"},{\"DKK_per_kWh\":1.17794,\"EUR_per_kWh\":0.15827,\"EXR\":7.442628,\"time_start\":\"2023-02-28T21:00:00+01:00\",\"time_end\":\"2023-02-28T22:00:00+01:00\"},{\"DKK_per_kWh\":1.14453,\"EUR_per_kWh\":0.15378,\"EXR\":7.442628,\"time_start\":\"2023-02-28T22:00:00+01:00\",\"time_end\":\"2023-02-28T23:00:00+01:00\"},{\"DKK_per_kWh\":1.0579,\"EUR_per_kWh\":0.14214,\"EXR\":7.442628,\"time_start\":\"2023-02-28T23:00:00+01:00\",\"time_end\":\"2023-03-01T00:00:00+01:00\"}]";

        List<ElectricityPriceDTO> electricityPrices = new List<ElectricityPriceDTO>();

        if (!string.IsNullOrEmpty(electricityPricesJson) && !string.IsNullOrWhiteSpace(electricityPricesJson))
        {
            electricityPrices = JsonSerializer.Deserialize<List<ElectricityPriceDTO>>(electricityPricesJson);

        }

        if (!electricityPrices.Any())
        {
            return electricityPrices;
        }
        foreach (var price in electricityPrices)
        {
            price.Location = location;
        }

        return electricityPrices;

    }

    private List<ElectricityPriceDTO> GetPricesBasedOnLocation(Locations location)
    {
        var jsonResult = GetPricesInDenmark(location);

        return MakePricesFromJson(jsonResult, location);
    }

    public List<ElectricityPriceDTO> GetWestPrices()
    {
        return GetPricesBasedOnLocation(Locations.WestDenmark);
    }

    public List<ElectricityPriceDTO> GetEastPrices()
    {
        return GetPricesBasedOnLocation(Locations.EastDenmark);
    }
}