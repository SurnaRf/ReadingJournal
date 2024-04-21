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
            var apiResponse = await httpClient.GetAsync($"{configuration["BookAPI"]}?q={Uri.EscapeDataString(query)}&limit=25");

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
                    string[] keyParts = apiBook.Key.Split('/');

                    if (keyParts.Length >= 3)
                    {
                        string keyAfterSecondSlash = keyParts[2];

                        Book book = new Book
                        {
                            Key = keyAfterSecondSlash,
                            Title = apiBook.Title,
                            Description = "",
                            Authors = apiBook.AuthorNames != null
                                ? apiBook.AuthorNames.Select(name =>
                                {
                                    string[] names = name.Split(' ');
                                    if (names.Length == 1)
                                    {
                                        return new Author(names[0], "Unknown");
                                    }
                                    else if (names.Length == 2)
                                    {
                                        return new Author { FirstName = names[0], LastName = names[1] };
                                    }
                                    else
                                    {
                                        return new Author("John", "Doe");
                                    }
                                }).ToList()
                                : new List<Author>() { new Author("John", "Doe") },
                            CoverUrl = GetCoverUrl(apiBook.CoverId)
                        };

                        books.Add(book);
                    }
                }
            }

            return books;
        }

        private string GetCoverUrl(int coverId)
        {
            if (coverId > 0)
            {
                return $"https://covers.openlibrary.org/b/id/{coverId}-L.jpg";
            }

            return "https://via.placeholder.com/150"; 
        }

        public async Task<ICollection<Book>> SearchBooks(string searchText)
        {
            throw new NotImplementedException();
        }
        private int? ParsePublishYear(List<int>? publishYearList)
        {
            return publishYearList?.FirstOrDefault();
        }

    }
}
