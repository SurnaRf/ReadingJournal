using BusinessLayer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer
{
    //To do: Write logic for UserContext.
    public class UserContext
    {
        UserManager<User> userManager;
        SignInManager<User> signInManager;
        ReadingJournalDbContext dbContext;

        public UserContext(ReadingJournalDbContext dbContext, 
            UserManager<User> userManager,
            SignInManager<User> signInManager)
        {
            this.dbContext = dbContext;
            this.userManager = userManager;
            this.signInManager = signInManager;            
        }

        public async Task SeedDataAsync(string adminPass, string adminEmail)
        {
            int roles = await dbContext.Roles.CountAsync();

            if (roles == 0)
            {
                await ConfigureRolesAsync();
            }

            int userRoles = await dbContext.UserRoles.CountAsync();

            if (userRoles == 0)
            {
                await ConfigureAdminAccountAsync(adminPass, adminEmail);
            }
        }

        private async Task ConfigureRolesAsync()
        {
            IdentityRole admin = new(Role.Administrator.ToString()) { NormalizedName = Role.Administrator.ToString().ToUpper()};
            IdentityRole user = new(Role.User.ToString()) { NormalizedName = Role.User.ToString().ToUpper() };
            IdentityRole unspecified = new(Role.Unspecified.ToString()) { NormalizedName = Role.Unspecified.ToString().ToUpper() };

            dbContext.Roles.Add(admin);
            dbContext.Roles.Add(user);
            dbContext.Roles.Add(unspecified);
            await dbContext.SaveChangesAsync();
        }

        private async Task ConfigureAdminAccountAsync(string password, string email)
        {
            User adminIdentityUser = await dbContext.Users.FirstOrDefaultAsync();

            if (adminIdentityUser == null)
            {
                adminIdentityUser = new("admin", "admin admin", "adminnn", "admin123");
                dbContext.Users.Add(adminIdentityUser);
                await dbContext.SaveChangesAsync();
            }

            await userManager.AddToRoleAsync(adminIdentityUser, Role.Administrator.ToString());
            await userManager.AddPasswordAsync(adminIdentityUser, password);
            await userManager.SetEmailAsync(adminIdentityUser, email);
        }

        public async Task CreateUserAsync(string username, string password, string email, string firstName, string lastName, int age, Role role)
        {
            try
            {
                User user = new(firstName, lastName, username, email, age, password, role);
                IdentityResult result = await userManager.CreateAsync(user, password);

                if (!result.Succeeded)
                {
                    throw new ArgumentException(result.Errors.First().Description);
                }

                if (role == Role.Administrator)
                {
                    await userManager.AddToRoleAsync(user, Role.Administrator.ToString());
                }
                else
                {
                    await userManager.AddToRoleAsync(user, Role.User.ToString());   
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<ClaimsPrincipal> LogInUserAsync(string username, string password)
        {
            try
            {
                User user = await userManager.FindByNameAsync(username);

                if (user == null)
                {
                    return null;
                }

                IdentityResult result = await userManager.PasswordValidators[0].ValidateAsync(userManager, user, password);

                if (result.Succeeded)
                {
                    User loggedUser = await dbContext.Users
                        .Include(u => u.Friends)
                        .Include(u => u.FavoriteGenres)
                        .Include(u => u.Shelves)
                        .FirstOrDefaultAsync(u => u.Id == user.Id);

                    return await signInManager.CreateUserPrincipalAsync(loggedUser);  
                }
                else
                {
                    return null;
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<ClaimsPrincipal> LogOutUserAsync(ClaimsPrincipal claimsPrincipal)
        {
            if (claimsPrincipal.Identity != null && claimsPrincipal.Identity.IsAuthenticated)
            {
                return new ClaimsPrincipal();
            }

            return claimsPrincipal;
        }

        public async Task<User> ReadUserAsync(string key, bool useNavigationalProperties = false)
        {
            try
            {
                if (useNavigationalProperties)
                {
                    return await userManager.Users
                        .Include(u => u.Friends)
                        .Include(u => u.FavoriteGenres)
                        .Include(u => u.Shelves)
                        .FirstOrDefaultAsync(u => u.Id == key);
                }

                return await userManager.FindByIdAsync(key);
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<IEnumerable<User>> ReadAllUsersAsync(bool useNavigationalProperties = false)
        {
            try
            {
                IQueryable<User> query = userManager.Users;

                if (useNavigationalProperties)
                {
                    query = query.Include(u => u.Friends)
                        .Include(u => u.FavoriteGenres)
                        .Include(u => u.Shelves);
                }

                return await query.ToListAsync();
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task UpdateUserAsync(User item, bool useNavigationalProperties = false)
        {
            try
            {
                User user = await ReadUserAsync(item.Id, useNavigationalProperties);

                if (user != null)
                {
                    user.NormalizedUserName = item.UserName.ToUpperInvariant();
                    user.Age = item.Age;
                    user.FirstName = item.FirstName;
                    user.LastName = item.LastName;

                    if (useNavigationalProperties)
                    {
                        List<User> friends = new();

                        foreach (User friend in item.Friends)
                        {
                            User friendFromDb = await dbContext.Users.FindAsync(friend.Id);

                            if (friendFromDb != null)
                            {
                                friends.Add(friendFromDb);
                            }
                            else
                            {
                                friends.Add(friend);
                            }
                        }

                        user.Friends = friends;

                        List<Genre> genres = new();

                        foreach (Genre genre in item.FavoriteGenres)
                        {
                            Genre genreFromDb = await dbContext.Genres.FindAsync(genre.Id);

                            if (genreFromDb != null)
                            {
                                genres.Add(genreFromDb);
                            }
                            else
                            {
                                genres.Add(genre);
                            }
                        }

                        user.FavoriteGenres = genres;

                        List<Shelf> shelves = new();

                        foreach (Shelf shelf in item.Shelves)
                        {
                            Shelf shelfFromDb = await dbContext.Shelves.FindAsync(shelf.Id);

                            if (shelfFromDb != null)
                            {
                                shelves.Add(shelfFromDb);
                            }
                            else
                            {
                                shelves.Add(shelf);
                            }
                        }

                        user.Shelves = shelves;
                    }

                    await dbContext.SaveChangesAsync(); 
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task DeleteUserAsync(string id)
        {
            try
            {
                User user = await ReadUserAsync(id);

                if (user == null)
                {
                    throw new ArgumentException("A user with the given id does not exist!");
                }

                dbContext.Users.Remove(user);
                await dbContext.SaveChangesAsync();
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task DeleteUserByNameAsync(string name)
        {
            try
            {
                User user = await FindUserByNameAsync(name);

                if (user == null)
                {
                    throw new InvalidOperationException("User not found!");
                }

                await userManager.DeleteAsync(user);
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<User> FindUserByNameAsync(string name, bool useNavigationalProperties = false)
        {
            try
            {
                if (useNavigationalProperties)
                {
                    return await dbContext.Users
                        .Include(u => u.Friends)
                        .Include(u => u.FavoriteGenres)
                        .Include(u => u.Shelves)
                        .FirstOrDefaultAsync(u => u.NormalizedUserName == name.ToUpper());
                }

                return await userManager.FindByNameAsync(name);
            }
            catch (Exception)
            {

                throw;
            }
        }

        public bool Exists(string key)
        {
            User user = dbContext.Users.Find(key);

            if (user == null)
            {
                return false;
            }

            return true;
        }
    }
}
