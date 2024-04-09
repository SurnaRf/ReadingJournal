using BusinessLayer;
using Microsoft.AspNetCore.Components.Authorization;
using ServiceLayer;
using System.Security.Claims;
using System.Security.Cryptography.Xml;

namespace PresentationLayer.Services
{
    public class UserService
    {
        private UserManager manager;

        public UserService(UserManager manager)
        {
            this.manager = manager;
        }
        
        public async Task<AuthenticationState> LogInUserAsync(string username, string password)
        {
            ClaimsPrincipal claimsPrincipal = await manager.LogInUserAsync(username, password);

            if (claimsPrincipal != null)
            {
                return new AuthenticationState(claimsPrincipal);
            }

            return null;
        }

        public async Task<ClaimsPrincipal> LogOutUserAsync(ClaimsPrincipal claimsPrincipal)
        {
            return await manager.LogOutUserAsync(claimsPrincipal);
        }       

        public async Task DeleteAsync(string key)
        {
            await manager.DeleteUserAsync(key);
        }

        public async Task DeleteUserByNameAsync(string name)
        {
            await manager.DeleteUserByNameAsync(name);
        }        

        public bool Exists(string key)
        {
            return manager.Exists(key);
        }

        public async Task<User> FindUserByNameAsync(string name, bool useNavigationalProperties = false)
        {
            return await manager.FindUserByNameAsync(name, useNavigationalProperties);
        }
    }
}
