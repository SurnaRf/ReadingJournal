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
	public class Shelf
	{
		[Key]
        public int Id { get; set; }

		[Required]
		public string Name { get; set; }

		[Required]
        public ShelfPurpose ShelfPurpose { get; set; }

        [Required]
        public User User { get; set; }

		[DisplayName("User")]
		[ForeignKey("User")]
		public string? UserId { get; set; }

		public List<Book> Books { get; set; }

        public Shelf()
        {
            Books = new();
        }

		public Shelf(string name, User user)
		{
			Name = name;
			User = user;
			Books = new();
		}
	}
}
