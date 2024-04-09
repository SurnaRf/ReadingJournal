using DataLayer;
using BusinessLayer;
using Microsoft.AspNetCore.Identity;

namespace ServiceLayer
{
    public static class ContextGenerator
    {
        private static ReadingJournalDbContext dbContext;
        private static AuthorContext authorContext;
        private static BookContext bookContext;
        private static EditionContext editionContext;
        private static GenreContext genreContext;
        private static PublisherContext publisherContext;
        private static ShelfContext shelfContext;
        private static UserContext userContext;
        private static UserManager<User> userManager;
        private static SignInManager<User> signInManager;


        public static ReadingJournalDbContext GetDbContext()
        {
            if (dbContext == null)
            {
                SetDbContext();
            }

            return dbContext;
        }

        public static void SetDbContext()
        {
            dbContext = new ReadingJournalDbContext();
        }

        public static AuthorContext GetAuthorContext()
        {
            if (authorContext == null)
            {
                SetAuthorContext();
            }

            return authorContext;
        }

        public static void SetAuthorContext()
        {
            authorContext = new AuthorContext(GetDbContext());
        }

        public static BookContext GetBookContext()
        {
            if (bookContext == null)
            {
                SetBookContext();
            }

            return bookContext;
        }

        public static void SetBookContext()
        {
            bookContext = new BookContext(GetDbContext());
        }

        public static EditionContext GetEditionContext()
        {
            if (editionContext == null)
            {
                SetEditionContext();
            }

            return editionContext;
        }

        public static void SetEditionContext()
        {
            editionContext = new EditionContext(GetDbContext());
        }

        public static GenreContext GetGenreContext()
        {
            if (genreContext == null)
            {
                SetGenreContext();
            }

            return genreContext;
        }

        public static void SetGenreContext()
        {
            genreContext = new GenreContext(GetDbContext());
        }

        public static PublisherContext GetPublisherContext()
        {
            if (publisherContext == null)
            {
                SetPublisherContext();
            }

            return publisherContext;
        }

        public static void SetPublisherContext()
        {
            publisherContext = new PublisherContext(GetDbContext());
        }

        public static ShelfContext GetShelfContext()
        {
            if (shelfContext == null)
            {
                SetShelfContext();
            }

            return shelfContext;
        }

        public static void SetShelfContext()
        {
            shelfContext = new ShelfContext(GetDbContext());
        }

        //public static UserContext GetUserContext()
        //{
        //    if (userContext == null)
        //    {
        //        SetUserContext();
        //    }

        //    return userContext;
        //}

        //public static void SetUserContext()
        //{
        //    userContext = new UserContext(GetDbContext(), userManager, signInManager);
        //}
    }
}