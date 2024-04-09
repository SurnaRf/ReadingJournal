using BusinessLayer;
using System.Net.Http;

namespace PresentationLayer.Services
{
	public interface IBookService
	{
		public Task<ICollection<Book>> GetBooks(string title);

		//public Task<ICollection<Book>> SearchBooks(string searchText);
	}
}
