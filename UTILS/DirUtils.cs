namespace ApiClient.UTILS
{
    public static class DirUtils
    {
        public static string CheckTrailingSlash(this string directory)
        {
            return directory.EndsWith(@"\") ? directory : directory + @"\";
        }
    }
}
