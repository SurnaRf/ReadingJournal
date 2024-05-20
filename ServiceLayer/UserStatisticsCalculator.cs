using BusinessLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceLayer
{
    public class UserStatisticsCalculator
    {
        public async Task<UserStatistics> CalculateStatisticsAsync(User user)
        {
            var userStatistics = new UserStatistics();

            if (user.Shelves == null || !user.Shelves.Any())
            {
                userStatistics.TotalBooksRead = 0;
                userStatistics.AverageRating = 0;
                userStatistics.BooksReadPerMonth = new Dictionary<string, int>();
                return userStatistics;
            }

            var readShelves = user.Shelves.Where(s => s.ShelfPurpose == ShelfPurpose.Read).ToList();
            if (!readShelves.Any())
            {
                userStatistics.TotalBooksRead = 0;
                userStatistics.AverageRating = 0;
                userStatistics.BooksReadPerMonth = new Dictionary<string, int>();
                return userStatistics;
            }

            var userBooksInReadShelves = user.UserBooks
                .Where(ub => readShelves.Any(s => s.Books.Any(b => b.Key == ub.BookId)))
                .ToList();

            if (!userBooksInReadShelves.Any())
            {
                userStatistics.TotalBooksRead = 0;
                userStatistics.AverageRating = 0;
                userStatistics.BooksReadPerMonth = new Dictionary<string, int>();
                return userStatistics;
            }

            userStatistics.TotalBooksRead = userBooksInReadShelves.Count;
            userStatistics.AverageRating = userBooksInReadShelves.Average(ub => ub.Rating ?? 0);

            userStatistics.BooksReadPerMonth = userBooksInReadShelves
                .Where(ub => ub.StartDate.HasValue)
                .GroupBy(ub => ub.StartDate.Value.ToString("yyyy-MM"))
                .ToDictionary(g => g.Key, g => g.Count());

            return userStatistics;
        }


    }
}
