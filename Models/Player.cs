namespace WebApplication1.Models
{
    public class Player(string name, int elo = 0)
    {
        public string Name { get; set; } = name;
        public int Elo { get; set; } = elo;
        public Guid Id { get; set; } = Guid.NewGuid();
        public int GamesPlayed { get; set; } = 0;
        public int GamesWon { get; set; } = 0;
        public int GamesLost => GamesPlayed - GamesWon;
    }
}