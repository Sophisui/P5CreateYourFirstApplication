using Microsoft.EntityFrameworkCore;

namespace P5CreateYourFirstApplication.Data
{
    public class SeedData
    {
        public static async Task Initialize(IServiceProvider serviceProvider)
        {
            using (var context = new ApplicationDbContext(
                serviceProvider.GetRequiredService<
                    DbContextOptions<ApplicationDbContext>>()))
            {

            };
        }
    }
}

