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
        public int Id { get; set; }

        [Required]
        public DateTime PublicationDate { get; set; }

		public int NumberOfPages { get; set; }

        [Required]
        public CoverType CoverType { get; set; }

        [Required]
        public Publisher Publisher { get; set; }

		[DisplayName("Publisher")]
		[ForeignKey("Publisher")]
        public int PublisherId{ get; set; }

        public Edition() 
		{

		}

		public Edition(DateTime publicationDate, int numberOfPages, CoverType coverType, Publisher publisher)
		{
			PublicationDate = publicationDate;
			NumberOfPages = numberOfPages;
			CoverType = coverType;
			Publisher = publisher;
		}
	}
}
