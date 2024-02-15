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

        private Dictionary<Guid, Player> ReadPlayersFromFile(string filePath)
        {
            IOHelpers.CreateIfDirectoryDoesntExist(filePath);

            try
            {
                var json = File.ReadAllText(filePath);
                var result = JsonSerializer.Deserialize<Player[]>(json);
                var dic = result?.ToDictionary(x => x.Id, x => x);
                return dic is null ? throw new InvalidDataException("Can't deserialize players.json.") : dic;
            }
            catch (FileNotFoundException)
            {
                Console.WriteLine($"{filePath} was not found.");
                return [];
            }
        }

        public bool TryAddPlayer(Player player)
        {
            if (Players.ContainsKey(player.Id))
            {
                return false;
            }
            lock (_lock)
            {
                Players.Add(player.Id, player);
                ToFileNoLock();
            }
            return true;
        }

        public void AdjustPlayersByRound(Round round)
        {
            if (!Players.TryGetValue(round.Player1.Id, out Player? player1) ||
                !Players.TryGetValue(round.Player2.Id, out Player? player2))
            {
                throw new ArgumentException("Failed to get winner/looser player for Round.");
            }

            if (round.IsDraw)
            {
                AdjustByDraw(player1, player2);
            }
            else
            {
                AdjustByRules(round, player1, player2);
            }

            ToFile();
        }

        public void RoundRemoved(Round removedRound)
        {
            if (!Players.TryGetValue(removedRound.Player1.Id, out Player? player1) ||
               !Players.TryGetValue(removedRound.Player2.Id, out Player? player2))
            {
                throw new ArgumentException("Failed to get winner/looser player for Round.");
            }

            var winner = removedRound.Player1.Colour == removedRound.WinnerColour ? player1 : player2;
            var looser = winner == player1 ? player2 : player1;

            winner.GamesWon--;
            looser.GamesLost--;
            ToFile();
        }

        private static void AdjustByDraw(Player player1, Player player2)
        {
            player1.GamesDrawed++;
            player2.GamesDrawed++;
        }

        private static void AdjustByRules(Round round, Player player1, Player player2)
        {
            if (round.WinnerColour == round.Player1.Colour)
            {
                player1.GamesWon++;
                player2.GamesLost++;
            }
            else
            {
                player2.GamesWon++;
                player1.GamesLost++;
            }
        }

        private void ToFileNoLock()
        {
            var json = JsonSerializer.Serialize(Players.Values, _jsonOptions);
            File.WriteAllText(PlayersFilePath, json);
        }

        private void ToFile()
        {
            lock (_lock)
            {
                ToFileNoLock();
            }
        }

        public string PlayersFilePath { get; }
        public Dictionary<Guid, Player> Players { get; set; }
    }
}