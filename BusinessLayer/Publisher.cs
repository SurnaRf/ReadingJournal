using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer
{
	public class Publisher
	{
		[Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        public List<Book> Books { get; set; }

        public Publisher() 
        {
            Books = new();
        }

		public Publisher(string name)
		{
			Name = name;
			Books = new();
		}
	}
}
