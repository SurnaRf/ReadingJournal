using BusinessLayer;
using DataLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceLayer
{
    public class PublisherManager
    {
        private readonly PublisherContext publisherContext;

        public PublisherManager(PublisherContext publisherContext)
        {
            this.publisherContext = publisherContext;
        }

        public async Task CreateAsync(Publisher publisher)
        {
            await publisherContext.CreateAsync(publisher);
        }

        public async Task<Publisher> ReadAsync(int key, bool useNavigationalProperties = false, bool isReadOnly = true)
        {
            return await publisherContext.ReadAsync(key, useNavigationalProperties, isReadOnly);
        }

        public async Task<ICollection<Publisher>> ReadAllAsync(bool useNavigationalProperties = false, bool isReadOnly = true)
        {
            return await publisherContext.ReadAllAsync(useNavigationalProperties, isReadOnly);
        }

        public async Task UpdateAsync(Publisher item, bool useNavigationalProperties = false)
        {
            await publisherContext.UpdateAsync(item, useNavigationalProperties);
        }

        public async Task DeleteAsync(int key)
        {
            await publisherContext.DeleteAsync(key);
        }
    }
}
