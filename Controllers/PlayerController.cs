using Microsoft.AspNetCore.Mvc;
using TournamentGuideServer.Models;

namespace TournamentGuideServer.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PlayerController : ControllerBase
    {
        private ILogger<PlayerController> _logger;

        private PlayerManager PlayerManager { get; }

        public PlayerController(ILogger<PlayerController> logger, PlayerManager playerManager)
        {
            _logger = logger;
            PlayerManager = playerManager;
        }

        [HttpGet("players", Name = "GetAllPlayers")]
        public IEnumerable<Player> GetAllPlayers()
        {
            return PlayerManager.Players;
        }

        [HttpPost("add-player", Name = "AddNewPlayer")]
        public IActionResult AddNewPlayer([FromBody] Player newPlayer)
        {
            try
            {
                if (newPlayer.Name == "string")
                {
                    // Swagger test response.
                    return CreatedAtRoute("AddNewPlayer", new { id = newPlayer.Id }, newPlayer);
                }

                PlayerManager.TryAddPlayer(newPlayer);

                // Return a success response - 201 Created
                return CreatedAtRoute("AddNewPlayer", new { id = newPlayer.Id }, newPlayer);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding new player");

                // Return a 500 Internal Server Error
                return StatusCode(500, "An error occurred while adding the player");
            }
        }
    }
}