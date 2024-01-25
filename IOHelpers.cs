namespace WebApplication1
{
    public static class IOHelpers
    {
        public static void CreateIfDirectoryDoesntExist(string filePath)
        {
            string? dir = Path.GetDirectoryName(filePath);
            if (!string.IsNullOrWhiteSpace(dir) && !Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }
        }
    }
}