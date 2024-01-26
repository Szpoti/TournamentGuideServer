using Microsoft.AspNetCore.Mvc;
using TournamentGuideServer.Controllers;
using TournamentGuideServer.Models;

namespace TournamentGuideServer.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class RoundController : ControllerBase
    {
        private ILogger<RoundController> _logger;
        private RoundManager RoundManager { get; }
        private PlayerManager PlayerManager { get; }

        public RoundController(ILogger<RoundController> logger, RoundManager roundManager, PlayerManager playerManager)
        {
            _logger = logger;
            RoundManager = roundManager;
            PlayerManager = playerManager;
        }

        [HttpGet("rounds", Name = "GetAllRounds")]
        public IEnumerable<Round> GetAllRounds()
        {
            return RoundManager.Rounds;
        }

        [HttpPost("add-round", Name = "AddNewRound")]
        public IActionResult AddNewRound([FromBody] Round newRound)
        {
            try
            {
                RoundManager.TryAddRound(newRound);

                // Return a success response - 201 Created
                return CreatedAtRoute("AddNewRound", new { id = newRound.Id }, newRound);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding new round.");

                // Return a 500 Internal Server Error
                return StatusCode(500, "An error occurred while adding the round.");
            }
        }
    }
}