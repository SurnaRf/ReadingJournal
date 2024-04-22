using BusinessLayer;
using DataLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceLayer
{
    public class ShelfManager
    {
        private readonly ShelfContext shelfContext;

        public ShelfManager(ShelfContext shelfContext)
        {
            this.shelfContext = shelfContext;
        }

        public async Task CreateAsync(Shelf shelf)
        {
            await shelfContext.CreateAsync(shelf);
        }

        public async Task<Shelf> ReadAsync(int key, bool useNavigationalProperties = false, bool isReadOnly = true)
        {
            return await shelfContext.ReadAsync(key, useNavigationalProperties, isReadOnly);
        }

        public async Task<ICollection<Shelf>> ReadAllAsync(bool useNavigationalProperties = false, bool isReadOnly = true)
        {
            return await shelfContext.ReadAllAsync(useNavigationalProperties, isReadOnly);
        }

        public async Task UpdateAsync(Shelf item, bool useNavigationalProperties = false)
        {
            await shelfContext.UpdateAsync(item, useNavigationalProperties);
        }

        public async Task DeleteAsync(int key)
        {
            await shelfContext.DeleteAsync(key);
        }

        public async Task<ICollection<Shelf>> GetShelvesForUserAsync(string userId, bool useNavigationalProperties = false)
        {
            return await shelfContext.GetShelvesForUserAsync(userId, useNavigationalProperties);
        }

        public async Task RateBookAsync(string userId, string bookId, int rating)
        {

            await shelfContext.RateBookAsync(userId, bookId, rating);
        }
    }
}
