using System.Data.SqlTypes;
using TournamentGuideServer.Models;

namespace TournamentGuideServer.Models
{
    public class Round(Player winner, Player loser, bool isDraw, Uri matchLink, string winnerColour, string loserColour) : IRound
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public Player Winner { get; set; } = winner;
        public Player Loser { get; set; } = loser;
        public bool IsDraw { get; set; } = isDraw;
        public Uri MatchLink { get; set; } = matchLink;
        public string WinnerColour { get; set; } = winnerColour;
        public string LoserColour { get; set; } = loserColour;
    }

    public class DrawRound(Player player1, Player player2, Uri matchLink, bool isDraw) : IRound
    {
        public Player Player1 { get; set; } = player1;
        public Player Player2 { get; set; } = player2;
        public bool IsDraw { get; set; } = isDraw;

        public Uri MatchLink { get; set; } = matchLink;
    }

    public interface IRound
    {
        public bool IsDraw { get; set; }

        Uri MatchLink { get; set; }
    }
}