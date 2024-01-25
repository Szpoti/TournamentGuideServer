using System.Runtime.CompilerServices;
using System.Text.Json;
using System.Text.Json.Serialization.Metadata;
using WebApplication1.Models;

namespace WebApplication1
{
    public class PlayerManager
    {
        public PlayerManager(string dataDir)
        {
            PlayersFilePath = Path.Combine(dataDir, "players.json");
            Players = ReadPlayersFromFile(PlayersFilePath);
        }

        private readonly object _lock = new();

        private HashSet<Player> ReadPlayersFromFile(string filePath)
        {
            IOHelpers.CreateIfDirectoryDoesntExist(filePath);

            try
            {
                var json = File.ReadAllText(filePath);
                var result = JsonSerializer.Deserialize<HashSet<Player>>(json);
                return result is null ? throw new InvalidDataException("Can't deserialize players.json.") : result;
            }
            catch (FileNotFoundException)
            {
                Console.WriteLine($"{filePath} was not found.");
                return [];
            }
        }

        public bool TryAddPlayer(Player player)
        {
            if (Players.Contains(player))
            {
                return false;
            }
            lock (_lock)
            {
                Players.Add(player);
                ToFileNoLock();
            }
            return true;
        }

        private void ToFileNoLock()
        {
            var json = JsonSerializer.Serialize(Players, new JsonSerializerOptions() { WriteIndented = true });
            File.WriteAllText(PlayersFilePath, json);
        }

        public string PlayersFilePath { get; }
        public HashSet<Player> Players { get; set; }
    }
}