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
        [Required(ErrorMessage = "Enter Key!")]
		public string Key { get; set; }

        [Required]
        public string Title { get; set; }

        [Required]
		public string Description { get; set; }

        [AllowNull]
        public string CoverUrl { get; set; }
        
        public List<Edition> Editions { get; set; }

        [Required]
        public List<Author> Authors { get; set; } 

        public List<Shelf> Shelves { get; set; }

        public List<Genre> Genres { get; set; }

        public Book()
        {
            Shelves = new();
            Genres = new();
            Authors = new();
            Editions = new();
        }

		public Book(string title, string description, string key)
		{
			Title = title;
			Description = description;
			Key = key;
			Shelves = new();
			Genres = new();
            Authors = new();
            Editions = new();
		}
	}
}
