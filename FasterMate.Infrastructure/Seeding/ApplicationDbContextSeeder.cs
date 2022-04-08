namespace FasterMate.Infrastructure.Seeding
{
    using FasterMate.Infrastrucutre.Data;

    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Logging;

    public class ApplicationDbContextSeeder : ISeeder
    {
        public async Task SeedAsync(ApplicationDbContext dbContext, IServiceProvider serviceProvider)
        {
            if (dbContext == null)
            {
                throw new ArgumentNullException($"{nameof(dbContext)} is null!");
            }

            if (serviceProvider == null)
            {
                throw new ArgumentNullException($"{nameof(serviceProvider)} is null");
            }

            var logger = serviceProvider
                .GetService<ILoggerFactory>()
                .CreateLogger(typeof(ApplicationDbContextSeeder));

            var seeders = new List<ISeeder>
            { new CountriesSeeder()};

            foreach (var seeder in seeders)
            {
                await seeder.SeedAsync(dbContext, serviceProvider);
                await dbContext.SaveChangesAsync();
                string message = $"Seeder {seeder.GetType().Name} done.";
                logger.LogInformation(message);
            }
        }
    }
}
