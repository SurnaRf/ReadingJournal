using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer
{
    public class UserStatistics
    {
        public int TotalBooksRead { get; set; }
        public double AverageRating { get; set; }
        public Dictionary<string, int> BooksReadPerMonth { get; set; }
    }
}
