public class FlowManager
{
    private ApiHandler ApiHandler { get; set; }
    private DatabaseContext DatabaseContext { get; set; }

    public FlowManager()
    {
        ApiHandler = new ApiHandler();
        DatabaseContext = new DatabaseContext();
    }

    public void StartCollectingPrices()
    {
        var westPrices = ApiHandler.GetWestPrices();

        var eastPrices = ApiHandler.GetEastPrices();

        bool westPricesExists = DatabaseContext.PricesAlreadyExists(westPrices);

        if (westPricesExists)
        {
            Console.WriteLine("prices from west denmark already exists");
        }
        else
        {
            Console.WriteLine("inserting new prices from west denmark");
            DatabaseContext.InsertElectricityPrices(westPrices);
        }

        
        bool eastPricesExists = DatabaseContext.PricesAlreadyExists(eastPrices);

        if (eastPricesExists)
        {
            Console.WriteLine("prices from east denmark already exists");
        }
        else
        {
            Console.WriteLine("inserting new prices from east denmark");
            DatabaseContext.InsertElectricityPrices(eastPrices);
        }
    }
}
