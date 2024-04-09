using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer
{
	public class Genre
	{
		[Key]
        public int Id { get; set; }

		[Required]
		public string Name { get; set; }

        public List<Book> Books { get; set; }

        public List<User> Users{ get; set; }

        public Genre()
        {
            Books = new();
            Users = new();
        }

		public Genre(string name)
		{
			Name = name;
			Books = new();
			Users = new();
		}
	}
}
