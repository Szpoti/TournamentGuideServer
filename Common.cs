using Microsoft.Extensions.Logging;
using System.Collections.Concurrent;
using System.Runtime.InteropServices;

namespace WebApplication1
{
    public static class Common
    {
        public static string GetDataDir()
        {
            var path = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            string fullpath = Path.Combine(path, "TournamentGuide");

            if (!File.Exists(fullpath))
            {
                Directory.CreateDirectory(fullpath);
            }

            return fullpath;
        }

        public static string GetProcessPath()
        {
            if (Environment.CurrentDirectory is { } path)
            {
                var fullpath = Path.Combine(path, "TournamentGuide");
                if (!File.Exists(fullpath))
                {
                    Directory.CreateDirectory(fullpath);
                }

                return fullpath;
            }
            throw new InvalidOperationException($"{nameof(Environment.ProcessPath)} doesn't exist.");
        }
    }
}