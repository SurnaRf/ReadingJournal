using BusinessLayer;
using DataLayer;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography.Xml;
using System.Text;
using System.Threading.Tasks;

namespace ServiceLayer
{
    public class UserManager
    {
        UserContext context;

        public UserManager(UserContext context)
        {
            this.context = context;
        }

        #region Seeding Data with this Project

        public async Task SeedDataAsync(string adminPass, string adminEmail)
        {
            await context.SeedDataAsync(adminPass, adminEmail);
        }

        #endregion

        #region CRUD

        public async Task CreateUserAsync(string username, string password, string email, string firstName, string lastName, Role role, int age)
        {
            await context.CreateUserAsync(username, password, email, firstName, lastName,  role, age);
        }

        public async Task<ClaimsPrincipal> LogInUserAsync(string username, string password)
        {
            return await context.LogInUserAsync(username, password);
        }

        public async Task<ClaimsPrincipal> LogOutUserAsync(ClaimsPrincipal claimsPrincipal)
        {
            return await context.LogOutUserAsync(claimsPrincipal);
        }

        public async Task<User> ReadUserAsync(string key, bool useNavigationalProperties = false)
        {
            return await context.ReadUserAsync(key, useNavigationalProperties);
        }

        public async Task<IEnumerable<User>>ReadAllUsersAsync(bool useNavigationalProperties = false)
        {
            return await context.ReadAllUsersAsync(useNavigationalProperties);
        }

        public async Task UpdateUserAsync(User item, bool useNavigationalProperties = false)
        {            
            await context.UpdateUserAsync(item, useNavigationalProperties);
        }

        public async Task DeleteUserAsync(string id)
        {
            await context.DeleteUserAsync(id);
        }

        public async Task DeleteUserByNameAsync(string name)
        {
            await context.DeleteUserByNameAsync(name);
        }
		public async Task ChangePassword(string userid, string password)
		{
			await context.ChangePassword(userid, password);
		}
		#endregion

		#region Find Methods

		public async Task<User> FindUserByNameAsync(string name, bool useNavigationalProperties = false)
        {
            return await context.FindUserByNameAsync(name, useNavigationalProperties);
        }

        public bool Exists(string key)
        {
            return context.Exists(key);
        }
        #endregion
    }
}
