using Microsoft.AspNetCore.Mvc;
using WebApplication1.Controllers;

namespace TournamentGuideServer.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class RoundController : ControllerBase
    {
        private ILogger<RoundController> _logger;
        private RoundManager RoundManager { get; }

        public RoundController(ILogger<RoundController> logger, RoundManager roundManager)
        {
            _logger = logger;
            RoundManager = roundManager;
        }
    }
}