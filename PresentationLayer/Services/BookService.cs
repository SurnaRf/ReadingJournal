using BusinessLayer;
using DataLayer;
using ServiceLayer;
using System.Collections.Generic;
using System.Net.Http;
using System.Runtime.InteropServices;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace PresentationLayer.Services
{
    public class BookService : IBookService
    {
        private readonly HttpClient httpClient;
        private readonly IConfiguration configuration;
        public BookService(HttpClient httpClient, IConfiguration configuration)
        {
            this.httpClient = httpClient;
            this.configuration = configuration;
        }

        public async Task<ICollection<Book>> GetBooks(string query = "")
        {
            var apiResponse = await httpClient.GetAsync($"{configuration["BookAPI"]}?q={Uri.EscapeDataString(query)}");

            if (!apiResponse.IsSuccessStatusCode)
            {
                throw new Exception("Failed to get data from OpenLibrary API");
            }

            var responseString = await apiResponse.Content.ReadAsStringAsync();
            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            var bookApiResponse = JsonSerializer.Deserialize<BookApiResponse>(responseString, options);

            var books = new List<Book>();

            if (bookApiResponse?.Docs != null) 
            {
                foreach (var apiBook in bookApiResponse.Docs)
                {
                    var book = new Book
                    {
                        ISBN = apiBook.Key,
                        Title = apiBook.Title,
                        Description = "", 
                        Edition = new Edition(),
                        Authors = apiBook.AuthorNames != null
                    ? apiBook.AuthorNames.Select(name => new Author { FirstName = name }).ToList()
                    : new List<Author>(),
                        CoverUrl = GetCoverUrl(apiBook.CoverId)
                    };

                    books.Add(book);
                }
            }

            return books;
        }

        private string GetCoverUrl(int coverId)
        {
            // Construct the URL for the cover image based on the CoverId
            if (coverId > 0)
            {
                return $"https://covers.openlibrary.org/b/id/{coverId}-L.jpg";
            }

            // Default cover URL if no valid CoverId is available
            return "https://via.placeholder.com/150"; // Placeholder image URL
        }

        public async Task<ICollection<Book>> SearchBooks(string searchText)
        {
            throw new NotImplementedException();
        }
    }
}
