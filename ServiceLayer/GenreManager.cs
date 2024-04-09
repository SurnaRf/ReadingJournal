using BusinessLayer;
using DataLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceLayer
{
    public class GenreManager
    {
        private readonly GenreContext genreContext;

        public GenreManager(GenreContext genreContext)
        {
            this.genreContext = genreContext;
        }

        public async Task CreateAsync(Genre genre)
        {
            await genreContext.CreateAsync(genre);
        }

        public async Task<Genre> ReadAsync(int key, bool useNavigationalProperties = false, bool isReadOnly = true)
        {
            return await genreContext.ReadAsync(key, useNavigationalProperties, isReadOnly);
        }

        public async Task<ICollection<Genre>> ReadAllAsync(bool useNavigationalProperties = false, bool isReadOnly = true)
        {
            return await genreContext.ReadAllAsync(useNavigationalProperties, isReadOnly);
        }

        public async Task UpdateAsync(Genre item, bool useNavigationalProperties = false)
        {
            await genreContext.UpdateAsync(item, useNavigationalProperties);
        }

        public async Task DeleteAsync(int key)
        {
            await genreContext.DeleteAsync(key);
        }
    }
}
