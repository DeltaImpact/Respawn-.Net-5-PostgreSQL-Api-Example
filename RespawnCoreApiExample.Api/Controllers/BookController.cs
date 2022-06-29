using System;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using RespawnCoreApiExample.DataAccess.Contexts;
using RespawnCoreApiExample.DataAccess.Extensions;
using RespawnCoreApiExample.Domain.Models.Dto;
using RespawnCoreApiExample.Domain.Models.Entities;

namespace RespawnCoreApiExample.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BookController : ControllerBase
{
    private readonly ILogger<BookController> _logger;
    private readonly ApplicationDbContext _applicationDbContext;
    private readonly IMapper _mapper;

    public BookController(
        ILogger<BookController> logger,
        ApplicationDbContext applicationDbContext,
        IMapper mapper
    )
    {
        _applicationDbContext = applicationDbContext;
        _mapper = mapper;
        _logger = logger;
    }

    [HttpGet]
    public IActionResult Get()
    {
        return Ok(_applicationDbContext.Books.ToList());
    }

    [HttpGet("{bookId:guid}")]
    public async Task<IActionResult> Get(Guid bookId)
    {
        var book = await _applicationDbContext.Books.GetById(bookId).FirstOrDefaultAsync();
        return book is null ? NotFound() : Ok(book);
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateBookDto bookDto)
    {
        var authors = _applicationDbContext.Authors.Where(e => bookDto.Authors.Contains(e.Id)).ToArray();
        var genres = _applicationDbContext.Genres.Where(e => bookDto.Genres.Contains(e.Name)).ToArray();
        if (authors.Length != bookDto.Authors.Count || genres.Length != bookDto.Genres.Count)
        {
            return BadRequest();
        }

        var savedBook = (await _applicationDbContext.Books.AddAsync(new Book
        {
            Name = bookDto.Name,
            Authors = authors,
            Genres = genres
        })).Entity;
        await _applicationDbContext.SaveChangesAsync();

        return Ok(_mapper.Map<BookDto>(savedBook));
    }

    [HttpDelete("{bookId:guid}")]
    public async Task<IActionResult> Delete(Guid bookId)
    {
        var bookToDelete = await _applicationDbContext.Books.GetById(bookId).FirstOrDefaultAsync();
        if (bookToDelete is null)
        {
            return NotFound();
        }

        var removedBook = _applicationDbContext.Books.Remove(bookToDelete).Entity;
        await _applicationDbContext.SaveChangesAsync();

        return Ok(_mapper.Map<BookDto>(removedBook));
    }
}