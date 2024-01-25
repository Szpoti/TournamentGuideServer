namespace TournamentGuideServer.Models
{
    public class Player(string name, int elo = 0)
    {
        public string Name { get; set; } = name;
        public int Elo { get; set; } = elo;
        public Guid Id { get; set; } = Guid.NewGuid();
        public int GamesWon { get; set; } = 0;
        public int GamesDrawed { get; set; } = 0;
        public int GamesLost { get; set; } = 0;
        public int GamesPlayed => GamesWon + GamesLost + GamesDrawed;
        public int Points => (GamesWon * 3) + (GamesDrawed * 1);
    }
}