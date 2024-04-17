using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer
{
    public class UserBook
    {
        public int UserId { get; set; }
        public User User { get; set; }

        public string BookId { get; set; }
        public Book Book { get; set; }

        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int Rating { get; set; }

        public UserBook()
        {
            
        }

        public UserBook(int userId, string bookId , DateTime startDate, DateTime endDate, int rating)
        {
            UserId = userId;
            BookId = bookId;
            StartDate = startDate;
            EndDate = endDate;
            Rating = rating;
        }
    }
}
