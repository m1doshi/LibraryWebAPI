using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json;
using System.Net;
using System.Net.Http.Json;
using System.Text;
using WebAPI.Core.DTOs;

namespace xUnit_tests.ApiTests.Controllers
{
    public class BookControllerTest : IClassFixture<WebApplicationFactory<Startup>>
    {
        private readonly HttpClient client;
        private readonly WebApplicationFactory<Startup> factory;
        public BookControllerTest(WebApplicationFactory<Startup> factory)
        {
            this.factory = factory;
            client = factory.CreateClient();
        }

        [Fact]
        public async Task GetAllBooks_ReturnsOkResult_WithListOfBooks()
        {
            var responce = await client.GetAsync("/bookController/getAllBooks?pageNumber=1&pageSize=5");
            Assert.Equal(HttpStatusCode.OK, responce.StatusCode);
            var books = await responce.Content.ReadFromJsonAsync<List<BookModel>>();
            Assert.NotNull(books);
            Assert.True(books.Count > 0);
        }

        [Fact]
        public async Task GetBookById_ReturnsOkResult_WithBookModel()
        {
            var responce = await client.GetAsync("/bookController/getBookById?bookId=20");
            Assert.Equal(HttpStatusCode.OK, responce.StatusCode);
            var book = await responce.Content.ReadFromJsonAsync<BookModel>();
            Assert.NotNull(book);
            Assert.Equal(20, book.BookID);
        }

        [Fact]
        public async Task GetBookById_EntityWasNotFound_ReturnsBadRequest()
        {
            var responce = await client.GetAsync("/bookController/getBookById?bookId=100");
            Assert.Equal(HttpStatusCode.BadRequest, responce.StatusCode);
        }

        [Fact]
        public async Task GetBookByISBN_ReturnsOkResult_WithBookModel()
        {
            var responce = await client.GetAsync("/bookController/getBookByISBN?isbn=111-1-1111-1111-1");
            Assert.Equal(HttpStatusCode.OK, responce.StatusCode);
            var book = await responce.Content.ReadFromJsonAsync<BookModel>();
            Assert.NotNull(book);
            Assert.Equal("111-1-1111-1111-1", book.ISBN);
        }

        [Fact]
        public async Task GetBookByISBN_EntityWasNotFound_ReturnsBadRequest()
        {
            var responce = await client.GetAsync("/bookController/getBookByISBN?isbn=222-1-1111-1111-1");
            Assert.Equal(HttpStatusCode.BadRequest, responce.StatusCode);
        }

        [Fact]
        public async Task AddNewBook_BookModelIsValid_ReturnsOkResult()
        {
            var newBook = new BookModel
            {
                ISBN = "555-3-3333-3333-3",
                BookTitle = "testTitle",
                Genre = "testGenre"
            };

            var content = new StringContent(JsonConvert.SerializeObject(newBook), Encoding.UTF8, "application/json");
            var responce = await client.PostAsync("/bookController/addNewBook", content);
            Assert.Equal(HttpStatusCode.OK, responce.StatusCode );
        }

        [Fact]
        public async Task AddNewBook_BookModelIsInvalid_ReturnsBadRequest()
        {
            var newBook = new BookModel
            {
                ISBN = "3122",
                BookTitle = "testTitle",
                Genre = "testGenre"
            };
            var content = new StringContent(JsonConvert.SerializeObject(newBook), Encoding.UTF8, "application/json");
            var responce = await client.PostAsync("/bookController/addNewBook", content);
            Assert.Equal(HttpStatusCode.BadRequest,responce.StatusCode);
        }

        [Fact]
        public async Task UpdateBook_UpdateBookRequestIsValid_ReturnsOkResult()
        {
            var request = new UpdateBookRequest
            {
                ISBN = "333-3-3333-3333-3",
                BookTitle = "NEWtestTitle",
                Genre = "testGenre"
            };
            var content = new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json");
            var responce = await client.PostAsync("/bookController/updateBook?bookId=21", content);
            Assert.Equal(HttpStatusCode.OK, responce.StatusCode);
        }

        [Fact]
        public async Task UpdateBook_UpdateBookRequestIsInvalid_ReturnsBadRequest()
        {
            var request = new UpdateBookRequest
            {
                ISBN = "3333",
                BookTitle = "NEWtestTitle",
                Genre = "testGenre"
            };
            var content = new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json");
            var responce = await client.PostAsync("/bookController/updateBook?bookId=21", content);
            Assert.Equal(HttpStatusCode.BadRequest, responce.StatusCode);
        }

        [Fact]
        public async Task DeleteBook_BookIdIsVaid_ReturnOkResult()
        {
            var responce = await client.DeleteAsync("/bookController/deleteBook?bookId=21");
            Assert.Equal(HttpStatusCode.OK, responce.StatusCode);
        }

        [Fact]
        public async Task DeleteBook_BookIdIsInvalid_ReturnsBadRequest()
        {
            var responce = await client.DeleteAsync("/bookController/deleteBook?bookId=2000");
            Assert.Equal(HttpStatusCode.BadRequest, responce.StatusCode);
        }

        [Fact]
        public async Task IssueBook_BookIsAvailable_ReturnsOkResult()
        {
            var bookId = 20;
            var userId = 1;
            var returnDate = DateTime.UtcNow.AddDays(7);
            var request = new IssueBookRequest
            {
                BookId = bookId,
                UserId = userId,
                ReturnDate = returnDate
            };
            var content = new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json");
            var response = await client.PostAsync("/bookController/issueBook", content);
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task IssueBook_ShouldReturnBadRequest_WhenBookUnavailable()
        {
            var bookId = 999;
            var userId = 1;
            var returnDate = DateTime.UtcNow.AddDays(7);
            var request = new IssueBookRequest
            {
                BookId = bookId,
                UserId = userId,
                ReturnDate = returnDate
            };
            var content = new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json");
            var response = await client.PostAsync("/bookController/issueBook", content);
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }
        [Fact]
        public async Task ReturnBook_ReturnsOkResult_WhenSuccess()
        {
            var response = await client.PostAsync("/bookController/returnBook?bookId=1020", null);
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            var result = await response.Content.ReadAsStringAsync();
            Assert.Equal("true", result);
        }

        [Fact]
        public async Task ReturnBook_ReturnsBadRequest_WhenBookAlreadyReturned()
        {
            var response = await client.PostAsync("/bookController/returnBook?bookId=20", null);
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode); 
        }

    }
}
