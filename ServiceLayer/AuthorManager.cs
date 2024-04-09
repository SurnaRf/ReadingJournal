using BusinessLayer;
using DataLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceLayer
{
    public class AuthorManager
    {
        private readonly AuthorContext authorContext;

        public AuthorManager(AuthorContext authorContext)
        {
            this.authorContext = authorContext;
        }

        public async Task CreateAsync(Author author)
        {
            await authorContext.CreateAsync(author);
        }

        public async Task<Author> ReadAsync(int key, bool useNavigationalProperties = false, bool isReadOnly = true)
        {
            return await authorContext.ReadAsync(key, useNavigationalProperties, isReadOnly);
        }

        public async Task<ICollection<Author>> ReadAllAsync(bool useNavigationalProperties = false, bool isReadOnly = true)
        {
            return await authorContext.ReadAllAsync(useNavigationalProperties, isReadOnly);
        }

        public async Task UpdateAsync(Author item, bool useNavigationalProperties = false)
        {
            await authorContext.UpdateAsync(item, useNavigationalProperties);
        }

        public async Task DeleteAsync(int key)
        {
            await authorContext.DeleteAsync(key);
        }
    }
}
