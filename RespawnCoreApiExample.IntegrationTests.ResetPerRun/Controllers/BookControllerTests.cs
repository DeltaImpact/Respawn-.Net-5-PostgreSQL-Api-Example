using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http.Json;
using System.Threading.Tasks;
using FluentAssertions;
using RespawnCoreApiExample.Domain.Constants;
using RespawnCoreApiExample.Domain.Models.Dto;
using RespawnCoreApiExample.Domain.Models.Entities;
using RespawnCoreApiExample.IntegrationTests.ResetPerRun.Fixtures;
using RespawnCoreApiExample.IntegrationTests.ResetPerRun.Utils;
using Xunit;

namespace RespawnCoreApiExample.IntegrationTests.ResetPerRun.Controllers
{
    [Collection("Database collection")]
    public class BookControllerTests : IntegrationTestFactory
    {
        private readonly DatabaseFixture fixture;
        private static string BaseUrl => "/api/Book";
        private static string BaseUrlById(Guid locationId) => $"/api/Book/{locationId}";

        #region Create

        [Fact]
        public async Task Create_BookWithoutAuthor_ShouldCreateBook()
        {
            const string bookName = "Crazy berry";

            var response = await Client.PostAsync(
                BaseUrl,
                new CreateBookDto
                {
                    Name = bookName,
                    Genres = new List<string> { BookGenres.Fantasy }
                }.Serialize());

            response.StatusCode.Should().Be(HttpStatusCode.OK);
            var book = await response.Content.DeserializeAsync<BookDto>();
            book.Name.Should().Be(bookName);
        }

        [Fact]
        public async Task Create_BookWithAuthor_ShouldCreateBook()
        {
            var author = (await Context.Authors.AddAsync(new Author
                {
                    FullName = "Oswald Orange",
                }
            )).Entity;
            await Context.SaveChangesAsync();
            const string bookName = "Crazy berry";

            var response = await Client.PostAsync(
                BaseUrl,
                new CreateBookDto
                {
                    Name = bookName,
                    Authors = new List<Guid> { author.Id },
                    Genres = new List<string> { BookGenres.Fantasy }
                }.Serialize()
            );

            response.StatusCode.Should().Be(HttpStatusCode.OK);
            var book = await response.Content.DeserializeAsync<BookDto>();
            book.Name.Should().Be(bookName);
        }

        #endregion

        #region Get

        [Fact]
        public async Task Get_ShouldReturnAllBooks()
        {
            Context.Books.AddRange(
                new Book
                {
                    Name = "Mighty mango 1",
                    Genres = Context.Genres.Take(1).ToList()
                },
                new Book
                {
                    Name = "Mighty mango 2",
                    Genres = Context.Genres.Take(1).ToList()
                },
                new Book
                {
                    Name = "Mighty mango 3",
                    Genres = Context.Genres.Take(1).ToList()
                }
            );
            await Context.SaveChangesAsync();

            var response = await Client.GetFromJsonAsync<List<BookDto>>(BaseUrl);

            response.Count.Should().BeGreaterOrEqualTo(3);
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

            var response = await Client.GetAsync(BaseUrlById(book.Id));

            response.StatusCode.Should().Be(HttpStatusCode.OK);
            book.Name.Should().Be(bookName);
        }

        [Fact]
        public async Task GetById_IfBookNotExist_ShouldReturnNotFound()
        {
            var response = await Client.GetAsync(BaseUrlById(Guid.Empty));

            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        #endregion

        #region Delete

        [Fact]
        public async Task Delete_IfBookExist_ShouldDeleteBook()
        {
            const string bookName = "Civil papaya";
            var book = (await Context.Books.AddAsync(new Book
            {
                Name = bookName
            })).Entity;
            await Context.SaveChangesAsync();

            var response = await Client.DeleteAsync(BaseUrlById(book.Id));

            response.StatusCode.Should().Be(HttpStatusCode.OK);
            Context.Books.Any(e => e.Name == bookName).Should().BeFalse();
            book.Name.Should().Be(bookName);
        }

        [Fact]
        public async Task Delete_IfBookNotExist_ShouldReturnNotFound()
        {
            var response = await Client.DeleteAsync(BaseUrlById(Guid.Empty));

            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        #endregion
    }
}