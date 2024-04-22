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
	public class ShelfContext : IDb<Shelf, int>
	{
		private readonly ReadingJournalDbContext dbContext;

        public ShelfContext(ReadingJournalDbContext dbContext)
        {
			this.dbContext = dbContext;
        }
        public async Task CreateAsync(Shelf item)
		{
			try
			{
                User userFromDb = await dbContext.Users.FindAsync(item.UserId);

                if (userFromDb != null)
                {
                    item.User = userFromDb;
                }

                dbContext.Shelves.Add(item);
				await dbContext.SaveChangesAsync();
			}
			catch (Exception)
			{

				throw;
			}
		}

		public async Task<Shelf> ReadAsync(int key, bool useNavigationalProperties = false, bool isReadOnly = true)
		{
			try
			{
				IQueryable<Shelf> query = dbContext.Shelves;

				if (useNavigationalProperties)
				{
					query = query.Include(sh => sh.Books).Include(sh => sh.User);
				}

				if (isReadOnly)
				{
					query = query.AsNoTrackingWithIdentityResolution();
				}

				return await query.FirstOrDefaultAsync(sh => sh.Id == key);
			}
			catch (Exception)
			{

				throw;
			}
		}

		public async Task<ICollection<Shelf>> ReadAllAsync(bool useNavigationalProperties = false, bool isReadOnly = true)
		{
			try
			{
				IQueryable<Shelf> query = dbContext.Shelves;

				if (useNavigationalProperties)
				{
					query = query.Include(sh => sh.Books).Include(sh => sh.User);
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

		public async Task UpdateAsync(Shelf item, bool useNavigationalProperties = false)
		{
			try
			{
				Shelf shelfFromDb = await ReadAsync(item.Id, useNavigationalProperties, false);

				if (shelfFromDb == null)
				{
					await CreateAsync(item);
				}

				dbContext.Entry(shelfFromDb).CurrentValues.SetValues(item);

				if (useNavigationalProperties)
				{
                    User userFromDb = await dbContext.Users.FindAsync(item.UserId);

                    if (userFromDb != null)
                    {
                        shelfFromDb.User = userFromDb;
                    }
					else
					{
						shelfFromDb.User = item.User;
					}

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

					shelfFromDb.Books = books;
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
				Shelf shelfFromDb = await ReadAsync(key, false, false);

				if (shelfFromDb == null)
				{
					throw new Exception("Shelf with that Id does not exist!");
				}

				dbContext.Shelves.Remove(shelfFromDb);
				await dbContext.SaveChangesAsync();
			}
			catch (Exception)
			{

				throw;
			}
		}

        public async Task<ICollection<Shelf>> GetShelvesForUserAsync(string userId, bool useNavigationalProperties = false)
        {
            try
            {
                IQueryable<Shelf> query = dbContext.Shelves.Where(sh => sh.UserId == userId);

                if (useNavigationalProperties)
                {
                    query = query.Include(sh => sh.Books).Include(sh => sh.User);
                }

                return await query.ToListAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task RateBookAsync(string userId, string bookId, int rating)
        {
            try
            {
                var userBook = await dbContext.UserBooks
                    .FirstOrDefaultAsync(ub => ub.UserId == userId && ub.BookId == bookId);

                if (userBook != null)
                {
                    // Update the rating
                    userBook.Rating = rating;
                    await dbContext.SaveChangesAsync();
                }
                else
                {
                    throw new Exception("UserBook entry not found. Unable to rate the book.");
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error rating the book.", ex);
            }
        }
    }
}
