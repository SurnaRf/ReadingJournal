using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer
{
	public class Edition
	{
        [Key]
        public string Id { get; set; }

        [Required]
        public DateTime PublicationDate { get; set; }

		public int NumberOfPages { get; set; }

        [Required]
        public CoverType CoverType { get; set; }

		[ForeignKey("Book")]
		[DisplayName("Book")]
		public string? BookId { get; set; }

		public Book? Book { get; set; }

        [Required]
        public Publisher Publisher { get; set; }

		[DisplayName("Publisher")]
		[ForeignKey("Publisher")]
        public int PublisherId{ get; set; }

        public Edition() 
		{
            Id = Guid.NewGuid().ToString();
        }

		public Edition(DateTime publicationDate, int numberOfPages, CoverType coverType, Publisher publisher, Book book = null)
		{
			Id = Guid.NewGuid().ToString();
			PublicationDate = publicationDate;
			NumberOfPages = numberOfPages;
			CoverType = coverType;
			Publisher = publisher;
			Book = book;
		}
	}
}
