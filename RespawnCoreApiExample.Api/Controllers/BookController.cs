using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using RespawnCoreApiExample.DataAccess.Contexts;
using RespawnCoreApiExample.DataAccess.Extensions;
using RespawnCoreApiExample.Domain.Db.Entities;

namespace RespawnCoreApiExample.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BookController : ControllerBase
    {
        public readonly RespawnExampleDbContext RespawnExampleDbContext;

        private readonly ILogger<BookController> _logger;

        public BookController(ILogger<BookController> logger, RespawnExampleDbContext respawnExampleDbContext)
        {
            RespawnExampleDbContext = respawnExampleDbContext;
            _logger = logger;
        }

        [HttpGet]
        public IActionResult Get()
        {
            return Ok(RespawnExampleDbContext.Books.ToList());
        }

        [HttpGet("{bookId:guid}")]
        public async Task<IActionResult> Get(Guid bookId)
        {
            var book = await RespawnExampleDbContext.Books.GetById(bookId).FirstOrDefaultAsync();
            return book is null ? NotFound() : Ok(book);
        }

        [HttpPost]
        public async Task<IActionResult> Create(Book book)
        {
            var savedBook = await RespawnExampleDbContext.Books.AddAsync(book);
            await RespawnExampleDbContext.SaveChangesAsync();
            return Ok(savedBook.Entity);
        }

        [HttpDelete("{bookId:guid}")]
        public async Task<IActionResult> Delete(Guid bookId)
        {
            var bookToDelete = await RespawnExampleDbContext.Books.GetById(bookId).FirstOrDefaultAsync();
            if (bookToDelete is null)
            {
                return NotFound();
            }
            
            var removedBook = RespawnExampleDbContext.Books.Remove(bookToDelete);
            await RespawnExampleDbContext.SaveChangesAsync();
            return Ok(removedBook.Entity);
        }
    }
}