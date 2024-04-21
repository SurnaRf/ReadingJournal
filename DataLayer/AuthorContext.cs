using BusinessLayer;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer
{
	public class AuthorContext : IDb<Author, int>
	{
		private readonly ReadingJournalDbContext dbContext;

        public AuthorContext(ReadingJournalDbContext dbContext)
        {
			this.dbContext = dbContext;
        }

        public async Task CreateAsync(Author item)
		{
			try
			{
				dbContext.Add(item);
				await dbContext.SaveChangesAsync();
			}
			catch (Exception)
			{

				throw;
			}
		}

		public async Task<Author> ReadAsync(int key, bool useNavigationalProperties = false, bool isReadOnly = true)
		{
			try
			{
				IQueryable<Author> query = dbContext.Authors;

				if (useNavigationalProperties)
				{
					query = query.Include(a => a.Books);
				}

				if (isReadOnly)
				{
					query = query.AsNoTrackingWithIdentityResolution();
				}

				return await query.FirstOrDefaultAsync(a => a.Id == key);
			}
			catch (Exception)
			{

				throw;
			}
		}

		public async Task<ICollection<Author>> ReadAllAsync(bool useNavigationalProperties = false, bool isReadOnly = true)
		{
			try
			{
				IQueryable<Author> query = dbContext.Authors;

				if (useNavigationalProperties)
				{
					query = query.Include(a => a.Books);
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

		public async Task UpdateAsync(Author item, bool useNavigationalProperties = false)
		{
			try
			{
				Author authorFromDb = await ReadAsync(item.Id, useNavigationalProperties, false);

				if (authorFromDb == null) 
				{ 
					await CreateAsync(item); 
				}

				dbContext.Entry(authorFromDb).CurrentValues.SetValues(item);

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

					authorFromDb.Books = books;
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
				Author authorFromDb = await ReadAsync(key, false, false);

				if (authorFromDb == null)
				{
					throw new Exception("Author with that Id does not exist!");
				}

				dbContext.Authors.Remove(authorFromDb);
				await dbContext.SaveChangesAsync();
			}
			catch (Exception)
			{

				throw;
			}
		}
	}
}
