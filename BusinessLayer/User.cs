using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace BusinessLayer
{
	public class User : IdentityUser
	{
		
		public string FirstName { get; set; }

        public string Password { get; set; }


        public string LastName { get; set; }

        public int? Age { get; set; }

        public Role Role{ get; set; }

        public List<User> Friends { get; set; }

        public List<Shelf> Shelves { get; set; }

        public List<Genre> FavoriteGenres{ get; set; }

		public List<FriendRequest> FriendRequests { get; set; }

        public List<UserBook> UserBooks { get; set; }

        public User()
        {
            Friends = new();
            Shelves = new();
			FavoriteGenres = new();
            FriendRequests = new();
            UserBooks = new();
        }

        public User(string username, string firstName, string lastName, string password) : base(username)
        {
            FirstName = firstName;
            LastName = lastName;
            Password = password;
            Friends = new();
            Shelves = new();
            FavoriteGenres = new();
            FriendRequests = new();
            UserBooks = new();
        }

        public User(string firstName, string lastName, string username, string email, int age, string password, Role role)
		{
            FirstName = firstName;
            LastName = lastName;
            UserName = username;
            NormalizedUserName = username.ToUpper();
			Email = email;
            NormalizedEmail = email.ToUpper();
			Age = age;
            Role = role;
            Password = password;
			Friends = new();
			Shelves = new();
			FavoriteGenres = new();
            FriendRequests = new();
            UserBooks = new();
		}

        public User(string id, string firstName, string lastName, string username, string email, int age, string password, Role role)
            : this(firstName, lastName, username, email, age, password, role)
        {
            Id = id;
        }
    }
}