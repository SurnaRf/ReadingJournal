using BusinessLayer;
using DataLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace ServiceLayer
{
    public class BookManager
    {
        private readonly BookContext bookContext;

        public BookManager(BookContext bookContext)
        {
            this.bookContext = bookContext;
        }

        public async Task CreateAsync(Book book)
        {
            await bookContext.CreateAsync(book);
        }

        public async Task<Book> ReadAsync(string key, bool useNavigationalProperties = false, bool isReadOnly = true)
        {
            return await bookContext.ReadAsync(key, useNavigationalProperties, isReadOnly);
        }

        public async Task<ICollection<Book>> ReadAllAsync(bool useNavigationalProperties = false, bool isReadOnly = true)
        {
            return await bookContext.ReadAllAsync(useNavigationalProperties, isReadOnly);
        }

        public async Task UpdateAsync(Book item, bool useNavigationalProperties = false)
        {
            await bookContext.UpdateAsync(item, useNavigationalProperties);
        }

        public async Task DeleteAsync(string key)
        {
            await bookContext.DeleteAsync(key);
        }

        public async Task UpdateUserBookAsync(UserBook item, bool useNavigationalProperties = false)
        {
            await bookContext.UpdateUserBookAsync(item, useNavigationalProperties);
        }

        public async Task<UserBook> ReadUserBookAsync(string userId, string bookKey, bool useNavigationalProperties = false, bool isReadOnly = true)
        {
            return await bookContext.ReadUserBookAsync(userId, bookKey, useNavigationalProperties, isReadOnly);
        }

        public async Task UpdateUserBookRatingAsync(string userId, string bookId, int rating)
        {          
           await bookContext.UpdateUserBookRatingAsync(userId, bookId, rating);                      
        }

        public async Task ShareBookAsync(string userId, string bookId, List<string> friendIds)
        {
            await bookContext.ShareBookAsync(userId, bookId, friendIds);
        }
    }
}
