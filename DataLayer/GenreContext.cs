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
	public class GenreContext : IDb<Genre, int>
	{
		private readonly ReadingJournalDbContext dbContext;

        public GenreContext(ReadingJournalDbContext dbContext)
        {
			this.dbContext = dbContext;
        }
        public async Task CreateAsync(Genre item)
		{
			try
			{
				dbContext.Genres.Add(item);
				await dbContext.SaveChangesAsync();
			}
			catch (Exception)
			{

				throw;
			}
		}

		public async Task<Genre> ReadAsync(int key, bool useNavigationalProperties = false, bool isReadOnly = true)
		{
			try
			{
				IQueryable<Genre> query = dbContext.Genres;

				if (useNavigationalProperties)
				{
					query = query.Include(g => g.Books).Include(g => g.Users);
				}

				if (isReadOnly)
				{
					query = query.AsNoTrackingWithIdentityResolution();
				}

				return await query.FirstOrDefaultAsync(g => g.Id == key);
			}
			catch (Exception)
			{

				throw;
			}
		}

		public async Task<ICollection<Genre>> ReadAllAsync(bool useNavigationalProperties = false, bool isReadOnly = true)
		{
			try
			{
				IQueryable<Genre> query = dbContext.Genres;

				if (useNavigationalProperties)
				{
					query = query.Include(g => g.Books).Include(g => g.Users);
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

		public async Task UpdateAsync(Genre item, bool useNavigationalProperties = false)
		{
			try
			{
				Genre genreFromDb = await ReadAsync(item.Id, useNavigationalProperties, false);

				if (genreFromDb == null)
				{
					await CreateAsync(item);
				}

				dbContext.Entry(genreFromDb).CurrentValues.SetValues(item);

				if (useNavigationalProperties)
				{
					List<Book> books = new(item.Books.Count);
					List<User> users = new(item.Users.Count);

					foreach (Book book in item.Books)
					{
						Book bookFromDb = await dbContext.Books.FindAsync(book.ISBN);

						if (bookFromDb == null)
						{
							books.Add(book);
						}
						else
						{
							books.Add(bookFromDb);
						}
					}

					foreach (User user in item.Users)
					{
						User userFromDb = await dbContext.Users.FindAsync(user.Id);

						if (userFromDb == null)
						{
							users.Add(user);
						}
						else
						{
							users.Add(userFromDb);
						}
					}

					genreFromDb.Books = books;
					genreFromDb.Users = users;
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
				Genre genreFromDb = await ReadAsync(key, false, false);

				if (genreFromDb is null)
				{
					throw new Exception("Genre with that Id does not exist!");
				}

				dbContext.Genres.Remove(genreFromDb);
				await dbContext.SaveChangesAsync();
			}
			catch (Exception)
			{

				throw;
			}
		}
	}
}
