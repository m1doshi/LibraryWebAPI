using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json;
using System.Net;
using System.Net.Http.Json;
using System.Text;
using WebAPI.Core.DTOs;

namespace xUnit_tests.ApiTests.Controllers
{
    public class AuthorControllerTest : IClassFixture<WebApplicationFactory<Startup>>
    {

        private readonly HttpClient client;
        private readonly WebApplicationFactory<Startup> factory;
      
        public AuthorControllerTest(WebApplicationFactory<Startup> factory)
        {
            this.factory = factory;
            client = factory.CreateClient();
        }

        [Fact]
        public async Task GetAllAuthors_ReturnsOkResult_WithListOfAuthors()
        {
            var response = await client.GetAsync("/authorController/getAllAuthors");
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            var authors = await response.Content.ReadFromJsonAsync<List<AuthorModel>>();
            Assert.NotNull(authors);
            Assert.True(authors.Count > 0);
        }

        [Fact]
        public async Task GetAuthorById_ReturnsOkResult_WithAuthorModel()
        {
            var responce = await client.GetAsync("/authorController/getAuthorById?authorId=1015");
            Assert.Equal(HttpStatusCode.OK, responce.StatusCode);
            var author = await responce.Content.ReadFromJsonAsync<AuthorModel>();
            Assert.NotNull(author);
            Assert.Equal(1015, author.AuthorID);
        }

        [Fact]
        public async Task GetAuthorById_EntityWasNotFound_ReturnsBadRequest()
        {
            var responce = await client.GetAsync("/authorController/getAuthorById?authorId=1111");
            Assert.Equal(HttpStatusCode.BadRequest, responce.StatusCode);
        }

        [Fact]
        public async Task AddNewAuthor_AuthorModelIsValid_ReturnsOkResult()
        {
            var newAuthor = new AuthorModel
            {
                FirstName = "testName",
                LastName = "testSurname"
            };
            var content = new StringContent(JsonConvert.SerializeObject(newAuthor), Encoding.UTF8, "application/json");
            var responce = await client.PostAsync("/authorController/addNewAuthor", content);
            Assert.Equal(HttpStatusCode.OK, responce.StatusCode);
        }

        [Fact]
        public async Task AddNewAuthor_AuthorModelIsInvalid_ReturnsBadRequest()
        {
            var newAuthor = new AuthorModel
            {
                FirstName = "",
                LastName = "testSurname"
            };
            var content = new StringContent(JsonConvert.SerializeObject(newAuthor), Encoding.UTF8, "application/json");
            var responce = await client.PostAsync("/authorController/addNewAuthor", content);
            Assert.Equal(HttpStatusCode.BadRequest,responce.StatusCode);
        }

        [Fact]
        public async Task UpdateAuthor_UpdateAuthorRequestIsValid_ReturnsOkResult()
        {
            var data = new UpdateAuthorRequest
            {
                FirstName = "NEWNEWnewTestName",
                LastName = "testSurname"
            };
            var content = new StringContent(JsonConvert.SerializeObject(data), Encoding.UTF8, "application/json");
            var responce = await client.PostAsync("/authorController/updateAuthor?authorId=1016", content);
            Assert.Equal(HttpStatusCode.OK, responce.StatusCode);
        }

        [Fact]
        public async Task UpdateAuthor_UpdateAuthorRequestIsInvalid_ReturnsBadRequest()
        {
            var data = new UpdateAuthorRequest
            {
                FirstName = "",
                LastName = "testSurname"
            };
            var content = new StringContent(JsonConvert.SerializeObject(data), Encoding.UTF8, "application/json");
            var responce = await client.PostAsync("/authorController/updateAuthor?authorId=1016", content);
            Assert.Equal(HttpStatusCode.BadRequest, responce.StatusCode);
        }

        [Fact]
        public async Task DeleteAuthor_AuthorIdIsValid_ReturnsOkResult()
        {
            var responce = await client.DeleteAsync("/authorController/deleteAuthor?authorId=1015");
            Assert.Equal(HttpStatusCode.OK, responce.StatusCode);
        }

        [Fact]
        public async Task DeleteAuthor_AuthorIdIsInvalid_ReturnsBadRequest()
        {
            var responce = await client.DeleteAsync("/authorController/deleteAuthor?authorId=999");
            Assert.Equal(HttpStatusCode.BadRequest, responce.StatusCode);
        }

        [Fact]
        public async Task GetAllBooksByAuthor_ReturnsOkResult_WithListOFBookModels()
        {
            var responce = await client.GetAsync("/authorController/getAllBooksByAuthor?authorId=1016");
            Assert.Equal(HttpStatusCode.OK,responce.StatusCode);
            var books = await responce.Content.ReadFromJsonAsync<List<BookModel>>();
            Assert.NotNull(books);
            Assert.Equal(1, books.Count);
        }

        [Fact]
        public async Task GetAllBooksByAuthor_CollectionIsEmpty_ReturnsBafRequest()
        {
            var responce = await client.GetAsync("/authorController/getAllBooksByAuthor?authorId=1017");
            Assert.Equal(HttpStatusCode.BadRequest, responce.StatusCode);
        }
    }
}