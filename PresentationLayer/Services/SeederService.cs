using ServiceLayer;

namespace PresentationLayer.Services
{
    public class SeederService
    {
        private readonly SeederManager manager;
        private readonly IConfiguration configuration;

        public SeederService(SeederManager manager, IConfiguration configuration)
        {
            this.manager = manager;
            this.configuration = configuration;
        }

        public async Task SeedData()
        {
            await manager.SeedData(configuration["DefaultAdminPassword"], configuration["DefaultAdminEmail"]);
        }
    }
}
