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
				//Edition editionFromDb = await dbContext.Editions.FindAsync(item.Edition.Id);

				//if (editionFromDb != null)
				//{
				//	item.Edition = editionFromDb;
				//}

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

                //var books = await query.ToListAsync(); // Retrieve all books asynchronously

                //// Example of handling null values (e.g., CoverUrl) before processing data
                //foreach (var book in books)
                //{
                //    if (book.CoverUrl == null)
                //    {
                //        book.CoverUrl = "default_cover.jpg"; // Set default cover URL or handle null case
                //    }
                //}

                //return books;
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
					//Edition editionFromDb = await dbContext.Editions.FindAsync(item.Edition.Id);

					//if (editionFromDb != null)
					//{
					//	bookFromDb.Edition = editionFromDb;
					//}

					//else
					//{
					//	bookFromDb.Edition = item.Edition;
					//}

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
	}
}
