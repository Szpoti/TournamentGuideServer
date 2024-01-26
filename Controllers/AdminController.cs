using Microsoft.AspNetCore.Mvc;
using TournamentGuideServer.Models;

namespace TournamentGuideServer.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AdminController
    {
        private readonly string? _apiKey;
        private readonly ILogger<AdminController> _logger;

        public AdminController(ILogger<AdminController> logger, IConfiguration configuration)
        {
            _logger = logger;
            _apiKey = configuration["API_KEY"];
        }

        [HttpGet("login", Name = "AdminLogin")]
        public bool Login(string apiKey)
        {
            return apiKey == _apiKey;
        }
    }
}