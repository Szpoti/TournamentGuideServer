using System.Text.Json;
using TournamentGuideServer.Models;

namespace TournamentGuideServer
{
    public class RoundManager
    {
        public RoundManager(string dataDir, PlayerManager playerManager)
        {
            RoundsFilePath = Path.Combine(dataDir, "rounds.json");
            Rounds = ReadRoundsFromFile(RoundsFilePath);
            PlayerManager = playerManager;
        }

        private readonly object _lock = new();
        private readonly JsonSerializerOptions _jsonOptions = new() { WriteIndented = true };

        private HashSet<IRound> ReadRoundsFromFile(string filePath)
        {
            IOHelpers.CreateIfDirectoryDoesntExist(filePath);

            try
            {
                var json = File.ReadAllText(filePath);
                var result = JsonSerializer.Deserialize<HashSet<IRound>>(json);
                return result is null ? throw new InvalidDataException("Can't deserialize rounds.json.") : result;
            }
            catch (FileNotFoundException)
            {
                Console.WriteLine($"{filePath} was not found.");
                return [];
            }
        }

        public bool TryAddRound(IRound round)
        {
            if (Rounds.Contains(round))
            {
                return false;
            }
            lock (_lock)
            {
                Rounds.Add(round);
                PlayerManager.AdjustPlayersByRound(round);
                return true;
            }
        }

        public string RoundsFilePath { get; }
        public HashSet<IRound> Rounds { get; set; }
        public PlayerManager PlayerManager { get; }
    }
}