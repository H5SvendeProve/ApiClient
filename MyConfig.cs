using System.Configuration;

public static class MyConfig
{
    public static Dictionary<string, string> GetAllAppSettings()
    {
        var tempDictionary = new Dictionary<string, string>();

        var allValues = ConfigurationManager.AppSettings;

        if (allValues.HasKeys())
        {
            foreach (var key in allValues.AllKeys)
            {
                var value = allValues.Get(key);
                tempDictionary.Add(key, value);
            }
        }
        return tempDictionary.Count > 0 ? tempDictionary : null;
    }
}
