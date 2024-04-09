using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceLayer
{
    public class SeederManager : IDisposable
    {
        private readonly IServiceScope _scope;

        private readonly UserManager manager;
        private bool seedDone = false;

        public SeederManager(IServiceProvider services)
        {
            _scope = services.CreateScope();
            this.manager = _scope.ServiceProvider.GetRequiredService<UserManager>();
        }

        public void Dispose()
        {
            _scope.Dispose();
        }

        public async Task SeedData(string adminPass, string adminEmail)
        {
            if (!seedDone)
            {
                await manager.SeedDataAsync(adminPass, adminEmail);
                seedDone = true;
            }
        }

    }
}
