using BusinessLayer;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer
{
	public class PublisherContext : IDb<Publisher, int>
	{
		private readonly ReadingJournalDbContext dbContext;

        public PublisherContext(ReadingJournalDbContext dbContext)
        {
			this.dbContext = dbContext;
        }
        public async Task CreateAsync(Publisher item)
		{
			try
			{
				dbContext.Publishers.Add(item);
				await dbContext.SaveChangesAsync();
			}
			catch (Exception)
			{

				throw;
			}
		}

		public async Task<ICollection<Publisher>> ReadAllAsync(bool useNavigationalProperties = false, bool isReadOnly = true)
		{
			try
			{
				IQueryable<Publisher> query = dbContext.Publishers;

				if (useNavigationalProperties)
				{
					query = query.Include(p => p.Books);
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

		public async Task<Publisher> ReadAsync(int key, bool useNavigationalProperties = false, bool isReadOnly = true)
		{
			try
			{
				IQueryable<Publisher> query = dbContext.Publishers;

				if (useNavigationalProperties)
				{
					query = query.Include(p => p.Books);
				}

				if (isReadOnly)
				{
					query = query.AsNoTrackingWithIdentityResolution();
				}

				return await query.FirstOrDefaultAsync(p => p.Id == key);
			}
			catch (Exception)
			{

				throw;
			}
		}

		public async Task UpdateAsync(Publisher item, bool useNavigationalProperties = false)
		{
			try
			{
				Publisher publisherFromDb = await ReadAsync(item.Id, useNavigationalProperties, false);

				if (publisherFromDb == null)
				{
					await CreateAsync(item);
				}

				dbContext.Entry(publisherFromDb).CurrentValues.SetValues(item);

				if (useNavigationalProperties)
				{
					List<Book> books = new(item.Books.Count);

					foreach (Book book in item.Books)
					{
						Book bookFromDb = await dbContext.Books.FindAsync(book.Key);

						if (bookFromDb == null)
						{
							books.Add(book);
						}
						else
						{
							books.Add(bookFromDb);
						}
					}
				
					publisherFromDb.Books = books;
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
				Publisher publisherFromDb = await ReadAsync(key, false, false);

				if (publisherFromDb == null)
				{
					throw new Exception("Publisher with that Id does not exist!");
				}

				dbContext.Publishers.Remove(publisherFromDb);
				await dbContext.SaveChangesAsync();
			}
			catch (Exception)
			{

				throw;
			}
		}
	}
}
