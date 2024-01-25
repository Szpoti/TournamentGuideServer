using WebApplication1.Models;

namespace TournamentGuideServer.Models
{
    public class Round
    {
        public Round(Player winner, Player looser, Uri matchLink)
        {
        }

        public Player Winner { get; set; }
        public Player Looser { get; set; }
        public Uri MatchLink { get; set; }
    }
}