using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using RespawnCoreApiExample.DataAccess.Contexts;

namespace RespawnCoreApiExample.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class GenreController : ControllerBase
    {
        public readonly RespawnExampleDbContext RespawnExampleDbContext;

        private readonly ILogger<GenreController> _logger;

        public GenreController(ILogger<GenreController> logger, RespawnExampleDbContext respawnExampleDbContext)
        {
            RespawnExampleDbContext = respawnExampleDbContext;
            _logger = logger;
        }

        [HttpGet]
        public IActionResult Get()
        {
            return Ok(RespawnExampleDbContext.Genres.ToList());
        }
    }
}