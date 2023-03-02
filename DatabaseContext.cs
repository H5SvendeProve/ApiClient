using ApiClient;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

public class DatabaseContext
{
    private readonly string DbConnectionString = ConfigurationManager.ConnectionStrings["DbConnection"].ConnectionString;

    private SqlDataReader? Reader;

    private const string DateFormat = "yyyy-MM-dd HH:mm:ss";

    // Mathias comment


    public bool HasConnection()
    {
        using SqlConnection con = new SqlConnection(DbConnectionString);

        bool connected = false;

        try
        {
            con.Open();

            connected = true;

        }
        catch (Exception e)
        {
            con?.Close();
            Console.WriteLine("unable to connect to database", e);
            throw;
        }

        finally
        {
            con?.Close();
        }
        return connected;
    }

    public bool InsertElectricityPrices(List<ElectricityPriceDTO> prices)
    {
       bool done = false;

        try
        {
            foreach (ElectricityPriceDTO price in prices)
            {
                done = InsertElectricityPrice(price);
            }
          
        }
        catch (Exception e)
        {
            Console.WriteLine("error occurced ", e);
            throw;
        }

        return done;
    }

    private bool InsertElectricityPrice(ElectricityPriceDTO electricityPrice)
    {
        using SqlConnection con = new SqlConnection(DbConnectionString);

        bool inserted = false;

        try
        {
            con.Open();
            SqlCommand cmd = new SqlCommand("InsertElectricityPrice", con)
            {
                CommandType = CommandType.StoredProcedure
            };

            cmd.Parameters.Add(new SqlParameter("@DKKPerKwh", electricityPrice.DKKPerKWh));
            cmd.Parameters.Add(new SqlParameter("@EURPerKWh", electricityPrice.EURPerKWh));
            cmd.Parameters.Add(new SqlParameter("@Exr", electricityPrice.Exr));
            cmd.Parameters.Add(new SqlParameter("@TimeStart", electricityPrice.TimeStart.ToString(DateFormat)));
            cmd.Parameters.Add(new SqlParameter("@TimeEnd", electricityPrice.TimeEnd.ToString(DateFormat)));
            cmd.Parameters.Add(new SqlParameter("@Location", electricityPrice.Location.ToString()));

            cmd.ExecuteNonQuery();

            inserted = true;
        }

        catch (Exception e)
        {
            Console.WriteLine("error inserting data \n ", e);
            con?.Close();
            throw;
        }

        finally
        {
            con?.Close();

        }

        return inserted;
    }

    private ElectricityPriceDTO GetFirstElectricityPrice(List<ElectricityPriceDTO> prices)
    {   
        return prices.FirstOrDefault();
    }

    public bool PricesAlreadyExists(List<ElectricityPriceDTO> prices)
    {
        bool exists = false;

        using SqlConnection con = new SqlConnection(DbConnectionString);

        try
        {
            con.Open();

            SqlCommand cmd = new SqlCommand("PriceExistsInDb", con)
            {
                CommandType = CommandType.StoredProcedure
            };

            var firstPrice = GetFirstElectricityPrice(prices);

            cmd.Parameters.Add(new SqlParameter("@TimeStart", firstPrice.TimeStart.ToString(DateFormat)));
            cmd.Parameters.Add(new SqlParameter("@TimeEnd", firstPrice.TimeEnd.ToString(DateFormat)));
            cmd.Parameters.Add(new SqlParameter("@Location", firstPrice.Location.ToString()));

            Reader = cmd.ExecuteReader();

            if (Reader.HasRows)
            {
                exists = true;
            }

        }
        catch (Exception)
        {
            Reader?.Close();
            con?.Close();
            throw;
        }

        finally 
        { 
            Reader?.Close();
            con?.Close(); 
        }

        return exists;
    }
}
