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

        private readonly string? _apiKey;

        public PlayerController(ILogger<PlayerController> logger, PlayerManager playerManager, IConfiguration configuration)
        {
            _logger = logger;
            PlayerManager = playerManager;
            _apiKey = configuration["API_KEY"];
        }

        [HttpGet("players", Name = "GetAllPlayers")]
        public IEnumerable<Player> GetAllPlayers()
        {
            return PlayerManager.Players.Values;
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

        [HttpPost("modify-player", Name = "ModifyExistingPlayer")]
        public IActionResult ModifyExistingPlayer([FromBody] PlayerModifyRequest request)
        {
            try
            {
                if (request.Player.Name == "string")
                {
                    // Swagger test response.
                    return CreatedAtRoute("ModifyExistingPlayer", new { id = request.Player.Id }, request.Player);
                }

                if(request.ApiKey != _apiKey)
                {
                    return StatusCode(500, "ApiKey not recognized.");
                }

                PlayerManager.ModifyPlayer(request.Player);

                // Return a success response - 201 Created
                return CreatedAtRoute("ModifyExistingPlayer", new { id = request.Player.Id }, request.Player);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error modifying existing player");

                // Return a 500 Internal Server Error
                return StatusCode(500, "An error occurred while modifying the player");
            }
        }
    }
}