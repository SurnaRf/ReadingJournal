using BusinessLayer;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer
{
	public class EditionContext : IDb<Edition, int>
	{
		private readonly ReadingJournalDbContext dbContext;

        public EditionContext(ReadingJournalDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public async Task CreateAsync(Edition item)
		{
			try
			{
				Publisher publisherFromDb = await dbContext.Publishers.FindAsync(item.PublisherId);

				if (publisherFromDb != null)
				{
					item.Publisher = publisherFromDb;
				}

				dbContext.Editions.Add(item);
				await dbContext.SaveChangesAsync();
			}
			catch (Exception)
			{

				throw;
			}
		}

		public async Task<Edition> ReadAsync(int key, bool useNavigationalProperties = false, bool isReadOnly = true)
		{
			try
			{
				IQueryable<Edition> query = dbContext.Editions;

				if (useNavigationalProperties)
				{
					query = query.Include(e => e.Publisher);
				}

				if (isReadOnly)
				{
					query = query.AsNoTrackingWithIdentityResolution();
				}

				return await query.FirstOrDefaultAsync(e => e.Id == key);
			}
			catch (Exception)
			{

				throw;
			}
		}
		public async Task<ICollection<Edition>> ReadAllAsync(bool useNavigationalProperties = false, bool isReadOnly = true)
		{
			try
			{
				IQueryable<Edition> query = dbContext.Editions;

				if (useNavigationalProperties)
				{
					query = query.Include(e => e.Publisher);
				}

				if (isReadOnly)
				{
					query = query.AsNoTrackingWithIdentityResolution();
				}

				return await query.ToListAsync();
			}
			catch (Exception)
			{

				throw;
			}
		}

		public async Task UpdateAsync(Edition item, bool useNavigationalProperties = false)
		{
			try
			{
				Edition editionFromDb = await ReadAsync(item.Id, useNavigationalProperties, false);

				if (editionFromDb == null)
				{
					await CreateAsync(editionFromDb);
					return;
				}

				dbContext.Entry(editionFromDb).CurrentValues.SetValues(item);

				if (useNavigationalProperties)
				{
					Publisher publisherFromDb = await dbContext.Publishers.FindAsync(item.PublisherId);

					if (publisherFromDb != null)
					{
						editionFromDb.Publisher = publisherFromDb;
					}
					else
					{
						editionFromDb.Publisher = item.Publisher;
					}
				}

				await dbContext.SaveChangesAsync();
			}
			catch (Exception)
			{

				throw;
			}
		}

		public async Task DeleteAsync(int key)
		{
			try
			{
				Edition editionFromDb = await ReadAsync(key, false, false);

				if (editionFromDb == null)
				{
					throw new ArgumentException("Edition with the given key does not exist!");
				}

				dbContext.Editions.Remove(editionFromDb);
				await dbContext.SaveChangesAsync();
			}
			catch (Exception)
			{

				throw;
			}
		}
	}
}
