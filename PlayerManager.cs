using System.Runtime.CompilerServices;
using System.Text.Json;
using System.Text.Json.Serialization.Metadata;
using TournamentGuideServer.Models;

namespace TournamentGuideServer
{
    public class PlayerManager
    {
        public PlayerManager(string dataDir)
        {
            PlayersFilePath = Path.Combine(dataDir, "players.json");
            Players = ReadPlayersFromFile(PlayersFilePath);
        }

        private readonly object _lock = new();
        private readonly JsonSerializerOptions _jsonOptions = new() { WriteIndented = true };

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
            var json = JsonSerializer.Serialize(Players, _jsonOptions);
            File.WriteAllText(PlayersFilePath, json);
        }

        public void AdjustPlayersByRound(IRound round)
        {
            switch (round)
            {
                case Round regularRound:
                    AdjustByRegularRound(regularRound);
                    break;

                case DrawRound drawRound:
                    AdjustByDrawRound(drawRound);
                    break;
            }
        }

        private void AdjustByDrawRound(DrawRound round)
        {
            if (!Players.TryGetValue(round.Player1, out Player? player1) ||
                !Players.TryGetValue(round.Player2, out Player? player2))
            {
                throw new ArgumentException("Failed to get winner/looser player for Round.");
            }

            player1.GamesDrawed++;
            player2.GamesDrawed++;
        }

        private void AdjustByRegularRound(Round round)
        {
            if (!Players.TryGetValue(round.Winner, out Player? winner) ||
                !Players.TryGetValue(round.Loser, out Player? loser))
            {
                throw new ArgumentException("Failed to get winner/looser player for Round.");
            }

            winner.GamesWon++;
            loser.GamesLost++;
        }

        public string PlayersFilePath { get; }
        public HashSet<Player> Players { get; set; }
    }
}