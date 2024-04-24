using BusinessLayer;
using System.Net.Http;

namespace ReadingJournal.Services
{
	public interface IBookService
	{
		public Task<ICollection<Book>> GetBooks(string title);
	}
}
