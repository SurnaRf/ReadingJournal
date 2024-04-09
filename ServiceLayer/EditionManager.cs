using BusinessLayer;
using DataLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceLayer
{
    public class EditionManager
    {
        private readonly EditionContext editionContext;

        public EditionManager(EditionContext editionContext)
        {
            this.editionContext = editionContext;
        }

        public async Task CreateAsync(Edition edition)
        {
            await editionContext.CreateAsync(edition);
        }

        public async Task<Edition> ReadAsync(int key, bool useNavigationalProperties = false, bool isReadOnly = true)
        {
            return await editionContext.ReadAsync(key, useNavigationalProperties, isReadOnly);
        }

        public async Task<ICollection<Edition>> ReadAllAsync(bool useNavigationalProperties = false, bool isReadOnly = true)
        {
            return await editionContext.ReadAllAsync(useNavigationalProperties, isReadOnly);
        }

        public async Task UpdateAsync(Edition item, bool useNavigationalProperties = false)
        {
            await editionContext.UpdateAsync(item, useNavigationalProperties);
        }

        public async Task DeleteAsync(int key)
        {
            await editionContext.DeleteAsync(key);
        }
    }
}
