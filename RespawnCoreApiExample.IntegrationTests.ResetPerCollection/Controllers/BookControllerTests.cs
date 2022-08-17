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
using RespawnCoreApiExample.IntegrationTests.ResetPerCollection.Utils;
using Xunit;

namespace RespawnCoreApiExample.IntegrationTests.ResetPerCollection.Controllers
{
    public class BookControllerTests : IClassFixture<BookControllerTestsFixture>
    {
        private readonly BookControllerTestsFixture _fixture;
        private static string BaseUrl => "/api/Book";
        private static string BaseUrlById(Guid locationId) => $"/api/Book/{locationId}";

        public BookControllerTests(BookControllerTestsFixture fixture)
        {
            _fixture = fixture;
            _fixture.Context.Books.WipeTable();
        }

        #region Create

        [Fact]
        public async Task Create_BookWithoutAuthor_ShouldCreateBook()
        {
            const string bookName = "Crazy berry";

            var response = await _fixture.Client.PostAsync(
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
            const string bookName = "Crazy berry";

            var response = await _fixture.Client.PostAsync(
                BaseUrl,
                new CreateBookDto
                {
                    Name = bookName,
                    Authors = new List<Guid> { _fixture.Author.Id },
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
            _fixture.Context.Books.AddRange(
                new Book
                {
                    Name = "Mighty mango 1",
                    Genres = _fixture.Context.Genres.Take(1).ToList()
                },
                new Book
                {
                    Name = "Mighty mango 2",
                    Genres = _fixture.Context.Genres.Take(1).ToList()
                },
                new Book
                {
                    Name = "Mighty mango 3",
                    Genres = _fixture.Context.Genres.Take(1).ToList()
                }
            );
            await _fixture.Context.SaveChangesAsync();

            var response = await _fixture.Client.GetFromJsonAsync<List<BookDto>>(BaseUrl);

            response.Count.Should().BeGreaterOrEqualTo(3);
        }

        [Fact]
        public async Task GetById_IfBookExist_ShouldReturnBook()
        {
            const string bookName = "Mighty mango";
            var book = _fixture.Context.Books.Add(new Book
            {
                Name = bookName
            }).Entity;
            await _fixture.Context.SaveChangesAsync();

            var response = await _fixture.Client.GetAsync(BaseUrlById(book.Id));

            response.StatusCode.Should().Be(HttpStatusCode.OK);
            book.Name.Should().Be(bookName);
        }

        [Fact]
        public async Task GetById_IfBookNotExist_ShouldReturnNotFound()
        {
            var response = await _fixture.Client.GetAsync(BaseUrlById(Guid.Empty));

            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        #endregion

        #region Delete

        [Fact]
        public async Task Delete_IfBookExist_ShouldDeleteBook()
        {
            const string bookName = "Civil papaya";
            var book = (await _fixture.Context.Books.AddAsync(new Book
            {
                Name = bookName
            })).Entity;
            await _fixture.Context.SaveChangesAsync();

            var response = await _fixture.Client.DeleteAsync(BaseUrlById(book.Id));

            response.StatusCode.Should().Be(HttpStatusCode.OK);
            _fixture.Context.Books.Any(e => e.Name == bookName).Should().BeFalse();
            book.Name.Should().Be(bookName);
        }

        [Fact]
        public async Task Delete_IfBookNotExist_ShouldReturnNotFound()
        {
            var response = await _fixture.Client.DeleteAsync(BaseUrlById(Guid.Empty));

            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        #endregion
    }
}