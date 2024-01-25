using Microsoft.AspNetCore.Mvc;
using TournamentGuideServer.Controllers;

namespace TournamentGuideServer.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class RoundController : ControllerBase
    {
        private ILogger<RoundController> _logger;
        private RoundManager RoundManager { get; }
        public PlayerManager PlayerManager { get; }

        public RoundController(ILogger<RoundController> logger, RoundManager roundManager, PlayerManager playerManager)
        {
            _logger = logger;
            RoundManager = roundManager;
            PlayerManager = playerManager;
        }
    }
}