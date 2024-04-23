using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer
{
    public class UserBook
    {
        public string UserId { get; set; }
        public User User { get; set; }

        public string BookId { get; set; }
        public Book Book { get; set; }

        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public int? Rating { get; set; }

        public UserBook()
        {
            
        }

        public UserBook(string userId, string bookId , DateTime? startDate = null, DateTime? endDate = null, int? rating = null)
        {
            UserId = userId;
            BookId = bookId;
            StartDate = startDate;
            EndDate = endDate;
            Rating = rating;
        }
    }
}
