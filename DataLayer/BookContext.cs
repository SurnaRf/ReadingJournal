using BusinessLayer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer
{
	public class BookContext : IDb<Book, string>
	{
		private readonly ReadingJournalDbContext dbContext;

        public BookContext(ReadingJournalDbContext dbContext)
        {
			this.dbContext = dbContext;
        }
        public async Task CreateAsync(Book item)
		{
			try
			{
				List<Author> authors = new();

				foreach (Author author in item.Authors)
				{
					Author authorFromDb = await dbContext.Authors.FindAsync(author.Id);

					if (authorFromDb != null)
					{
						authors.Add(authorFromDb);
					}

					else
					{
						authors.Add(author);
					}
				}
                List<Edition> editions = new();

                foreach (Edition edition in item.Editions)
                {
                    Edition editionFromDb = await dbContext.Editions.FindAsync(edition.Id);

                    if (editionFromDb != null)
                    {
                        editions.Add(editionFromDb);
                    }

                    else
                    {
                        editions.Add(edition);
                    }
                }
                item.Authors = authors;
                item.Editions = editions;

				dbContext.Books.Add(item);
				await dbContext.SaveChangesAsync();
			}
			catch (Exception)
			{

				throw;
			}
		}

		public async Task<Book> ReadAsync(string key, bool useNavigationalProperties = false, bool isReadOnly = true)
		{
			try
			{
				IQueryable<Book> query = dbContext.Books;

				if (useNavigationalProperties)
				{
					query = query.Include(b => b.Genres)
								.Include(b => b.Authors)
								.Include(b => b.Shelves)
								.Include(b => b.Editions);
				}

				if (isReadOnly)
				{
					query = query.AsNoTrackingWithIdentityResolution();
				}

				return await query.FirstOrDefaultAsync(b => b.Key == key);
			}
			catch (Exception)
			{

				throw;
			}
		}
		
		public async Task<ICollection<Book>> ReadAllAsync(bool useNavigationalProperties = false, bool isReadOnly = true)
		{
			try
			{
                IQueryable<Book> query = dbContext.Books;

                if (useNavigationalProperties)
                {
                    query = query.Include(b => b.Genres)
                                .Include(b => b.Authors)
                                .Include(b => b.Shelves)
                                .Include(b => b.Editions);
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

		public async Task UpdateAsync(Book item, bool useNavigationalProperties = false)
		{
			try
			{
				Book bookFromDb = await ReadAsync(item.Key, useNavigationalProperties, false);

				if (bookFromDb == null)
				{
					CreateAsync(item);
					return;
				}

                dbContext.Entry(bookFromDb).CurrentValues.SetValues(item);

				if (useNavigationalProperties)
				{
                    List<Author> authors = new();

                    foreach (Author author in item.Authors)
                    {
                        Author authorFromDb = await dbContext.Authors.FindAsync(author.Id);

                        if (authorFromDb != null)
                        {
                            authors.Add(authorFromDb);
                        }

                        else
                        {
                            authors.Add(author);
                        }
                    }

                    List<Shelf> shelves = new();

                    foreach (Shelf shelf in item.Shelves)
                    {
                        Shelf shelfFromDb = await dbContext.Shelves.FindAsync(shelf.Id);

                        if (shelfFromDb != null)
                        {
                            shelves.Add(shelfFromDb);
                        }

                        else
                        {
                            shelves.Add(shelf);
                        }
                    }

                    List<Genre> genres = new();

                    foreach (Genre genre in item.Genres)
                    {
                        Genre genreFromDb = await dbContext.Genres.FindAsync(genre.Id);

                        if (genreFromDb != null)
                        {
                            genres.Add(genreFromDb);
                        }

                        else
                        {
                            genres.Add(genre);
                        }
                    }

					List<Edition> editions = new();

                    foreach (Edition edition in item.Editions)
                    {
                        Edition editionFromDb = await dbContext.Editions.FindAsync(edition.Id);

                        if (editionFromDb != null)
                        {
                            editions.Add(editionFromDb);
                        }

                        else
                        {
                            editions.Add(edition);
                        }
                    }

                    bookFromDb.Authors = authors;
					bookFromDb.Shelves = shelves;
					bookFromDb.Genres = genres;
					bookFromDb.Editions = editions;
                }

				await dbContext.SaveChangesAsync();
            }
            catch (Exception)
			{

				throw;
			}
		}
		
		public async Task DeleteAsync(string key)
		{
			try
			{
				Book bookFromDb = await ReadAsync(key, false, false);

				if (bookFromDb == null)
				{
					throw new ArgumentException("A book with the given ISBN does not exist!");
				}

				dbContext.Books.Remove(bookFromDb);
				await dbContext.SaveChangesAsync();
			}
			catch (Exception)
			{

				throw;
			}
		}

        public async Task<UserBook> ReadUserBookAsync(string userId, string bookKey, bool useNavigationalProperties = false, bool isReadOnly = true)
        {
            try
            {
                IQueryable<UserBook> query = dbContext.UserBooks;

                if (useNavigationalProperties)
                {
                    query = query.Include(ub => ub.User)
                                 .Include(ub => ub.Book);
                }

                if (isReadOnly)
                {
                    query = query.AsNoTrackingWithIdentityResolution();
                }

                return await query.FirstOrDefaultAsync(ub => ub.UserId == userId && ub.BookId == bookKey);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task UpdateUserBookAsync(UserBook item, bool useNavigationalProperties = false)
        {
            try
            {
                UserBook userBookFromDb = await ReadUserBookAsync(item.UserId, item.BookId, useNavigationalProperties, false);

                if (userBookFromDb == null)
                {
                    throw new Exception("UserBook not found");
                }

                userBookFromDb.Rating = item.Rating;
                userBookFromDb.StartDate = item.StartDate;
                userBookFromDb.EndDate = item.EndDate;

                dbContext.UserBooks.Update(userBookFromDb);
                await dbContext.SaveChangesAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task UpdateUserBookRatingAsync(string userId, string bookId, int rating)
        {
            try
            {
                var userBook = await dbContext.UserBooks
                    .FirstOrDefaultAsync(ub => ub.UserId == userId && ub.BookId == bookId);

                if (userBook != null)
                {
                    userBook.Rating = rating;
                    await dbContext.SaveChangesAsync();
                }
                else
                {
                    throw new Exception("UserBook entry not found. Unable to update rating.");
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error updating book rating.", ex);
            }
        }

        public async Task ShareBookAsync(string userId, string bookId, List<string> friendIds)
        {
            try
            {
                foreach (var friendId in friendIds)
                {
                    var userBook = new UserBook
                    {
                        UserId = friendId,
                        BookId = bookId,
                    };
                    await dbContext.UserBooks.AddAsync(userBook);
                }
                await dbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {

            }           
        }

    }
}
