using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using RespawnCoreApiExample.Domain;
using RespawnCoreApiExample.Domain.Db.Entities;
using RespawnCoreApiExample.IntegrationTests.Extensions;
using RespawnCoreApiExample.IntegrationTests.Fixtures;
using Xunit;

namespace RespawnCoreApiExample.IntegrationTests.Controllers
{
    public class BookControllerTests : IntegrationTest
    {
        public BookControllerTests(ApiWebApplicationFactory fixture) : base(fixture)
        {
            SetupDatabaseAsync().Wait();
        }

        [Fact]
        public async Task Get_ShouldReturnAllBooks()
        {
            var response = await Client.GetAsync("/api/book");

            response.StatusCode.Should().Be(HttpStatusCode.OK);
            var books = await response.Content.DeserializeAsync<List<Book>>();
            books.Count.Should().Be(2);
        }

        [Fact]
        public async Task Create_ShouldCreateBook()
        {
            const string bookName = "Crazy berry";
            
            var response = await Client.PostAsync("/api/book", new Book {Name = bookName}.Serialize());

            response.StatusCode.Should().Be(HttpStatusCode.OK);
            Context.Books.Count().Should().Be(3);
            var book = await response.Content.DeserializeAsync<Book>();
            book.Name.Should().Be(bookName);
        }

        [Fact]
        public async Task GetById_IfBookExist_ShouldReturnBook()
        {
            const string bookName = "Mighty mango";
            var book = Context.Books.Add(new Book
            {
                Name = bookName
            }).Entity;
            await Context.SaveChangesAsync();
            
            var response = await Client.GetAsync($"/api/book/{book.Id}");
            
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            Context.Books.Count().Should().Be(3);
            book.Name.Should().Be(bookName);
        }

        [Fact]
        public async Task GetById_IfBookNotExist_ShouldReturnNotFound()
        {
            var response = await Client.GetAsync($"/api/book/{Guid.Empty}");
            
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
            Context.Books.Count().Should().Be(2);
        }

        [Fact]
        public async Task Delete_IfBookExist_ShouldDeleteBook()
        {
            const string bookName = "Civil papaya";
            var book = Context.Books.Add(new Book
            {
                Name = bookName
            }).Entity;
            await Context.SaveChangesAsync();
            
            var response = await Client.DeleteAsync($"/api/book/{book.Id}");
            
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            Context.Books.Any(e => e.Name == bookName).Should().BeFalse();
            Context.Books.Count().Should().Be(2);
            book.Name.Should().Be(bookName);
        }

        [Fact]
        public async Task Delete_IfBookNotExist_ShouldReturnNotFound()
        {
            var response = await Client.DeleteAsync($"/api/book/{Guid.Empty}");
            
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
            Context.Books.Count().Should().Be(2);
        }

        private async Task SetupDatabaseAsync()
        {
            var mysteryGenre = await Context.Genres.FirstOrDefaultAsync(e => e.Name == BookGenres.Mystery);
            var fantasyGenre = await Context.Genres.FirstOrDefaultAsync(e => e.Name == BookGenres.Fantasy);
            var contemporaryFictionGenre =
                await Context.Genres.FirstOrDefaultAsync(e => e.Name == BookGenres.ContemporaryFiction);

            Context.Books.AddRange(new Book
                {
                    Name = "Mysterious orange",
                    Genres = new List<Genre>
                    {
                        mysteryGenre,
                        fantasyGenre
                    }
                },
                new Book
                {
                    Name = "Brave potato",
                    Genres = new List<Genre>
                    {
                        mysteryGenre,
                        contemporaryFictionGenre,
                        fantasyGenre
                    }
                }
            );
            await Context.SaveChangesAsync();
        }
    }
}