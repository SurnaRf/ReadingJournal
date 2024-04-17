using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer
{
	public class Book
	{
		[Key] 
        [Required(ErrorMessage = "Enter ISBN!")]
        [StringLength(13, MinimumLength = 13, ErrorMessage = "ISBN must be exactly 13 symbols!")]
		public string ISBN { get; set; }

        [Required]
        public string Title { get; set; }

        [Required]
		public string Description { get; set; }

        [AllowNull]
        public string CoverUrl { get; set; }

        [Required]
        public Edition Edition { get; set; }

        [DisplayName("Edition")]
        [ForeignKey("Edition")]
        public int EditionId { get; set; }

        [Required]
        public List<Author> Authors { get; set; } 

        public List<Shelf> Shelves { get; set; }

        public List<Genre> Genres { get; set; }

        public Book()
        {
            Shelves = new();
            Genres = new();
            Authors = new();
        }

		public Book(string title, string description, string iSBN, Edition edition)
		{
			Title = title;
			Description = description;
			ISBN = iSBN;
			Edition = edition;
			Shelves = new();
			Genres = new();
            Authors = new();
		}
	}
}
