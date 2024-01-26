using System.Data.SqlTypes;
using TournamentGuideServer.Models;

namespace TournamentGuideServer.Models
{
    public class Round(PlayerInfo player1, PlayerInfo player2, bool isDraw, Uri matchLink, string winnerColour, string loserColour)
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public PlayerInfo Player1 { get; set; } = player1;
        public PlayerInfo Player2 { get; set; } = player2;
        public string WinnerColour { get; set; } = winnerColour;
        public string LoserColour { get; set; } = loserColour;
        public bool IsDraw { get; set; } = isDraw;
        public Uri MatchLink { get; set; } = matchLink;

        public override bool Equals(object? obj) => Equals(obj as Round);

        public bool Equals(Round? obj) => this.Id == obj?.Id;
    }

    public record PlayerInfo(Guid Id, string Colour);
}