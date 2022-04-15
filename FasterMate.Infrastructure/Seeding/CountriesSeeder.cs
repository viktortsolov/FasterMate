namespace FasterMate.Infrastructure.Seeding
{
    using FasterMate.Infrastructure.Data;
    using FasterMate.Infrastrucutre.Data;
    using System.Diagnostics.CodeAnalysis;

    [ExcludeFromCodeCoverage]
    public class CountriesSeeder : ISeeder
    {
        public async Task SeedAsync(ApplicationDbContext dbContext, IServiceProvider serviceProvider)
        {
            if (dbContext.Countries.Any())
            {
                return;
            }

            await dbContext.Countries.AddAsync(new Country { Name = "Russia" });
            await dbContext.Countries.AddAsync(new Country { Name = "Germany" });
            await dbContext.Countries.AddAsync(new Country { Name = "United Kingdom" });
            await dbContext.Countries.AddAsync(new Country { Name = "France" });
            await dbContext.Countries.AddAsync(new Country { Name = "Italy" });
            await dbContext.Countries.AddAsync(new Country { Name = "Spain" });
            await dbContext.Countries.AddAsync(new Country { Name = "Ukraine" });
            await dbContext.Countries.AddAsync(new Country { Name = "Poland" });
            await dbContext.Countries.AddAsync(new Country { Name = "Romania" });
            await dbContext.Countries.AddAsync(new Country { Name = "Netherlands" });
            await dbContext.Countries.AddAsync(new Country { Name = "Belgium" });
            await dbContext.Countries.AddAsync(new Country { Name = "Czech Republic" });
            await dbContext.Countries.AddAsync(new Country { Name = "Greece" });
            await dbContext.Countries.AddAsync(new Country { Name = "Portugal" });
            await dbContext.Countries.AddAsync(new Country { Name = "Sweeden" });
            await dbContext.Countries.AddAsync(new Country { Name = "Hungary" });
            await dbContext.Countries.AddAsync(new Country { Name = "Belarus" });
            await dbContext.Countries.AddAsync(new Country { Name = "Austria" });
            await dbContext.Countries.AddAsync(new Country { Name = "Serbia" });
            await dbContext.Countries.AddAsync(new Country { Name = "Switzerland" });
            await dbContext.Countries.AddAsync(new Country { Name = "Bulgaria" });
            await dbContext.Countries.AddAsync(new Country { Name = "Denmark" });
            await dbContext.Countries.AddAsync(new Country { Name = "Finland" });
            await dbContext.Countries.AddAsync(new Country { Name = "Slovakia" });
            await dbContext.Countries.AddAsync(new Country { Name = "Norway" });
            await dbContext.Countries.AddAsync(new Country { Name = "Ireland" });
            await dbContext.Countries.AddAsync(new Country { Name = "Croatia" });
            await dbContext.Countries.AddAsync(new Country { Name = "Moldova" });
            await dbContext.Countries.AddAsync(new Country { Name = "Bosnia and Herzegovina" });
            await dbContext.Countries.AddAsync(new Country { Name = "Albania" });
            await dbContext.Countries.AddAsync(new Country { Name = "Lithuania" });
            await dbContext.Countries.AddAsync(new Country { Name = "North Macedonia" });
            await dbContext.Countries.AddAsync(new Country { Name = "Slovenia" });
            await dbContext.Countries.AddAsync(new Country { Name = "Latvia" });
            await dbContext.Countries.AddAsync(new Country { Name = "Estonia" });
            await dbContext.Countries.AddAsync(new Country { Name = "Montenegro" });
            await dbContext.Countries.AddAsync(new Country { Name = "Luxembourg" });
            await dbContext.Countries.AddAsync(new Country { Name = "Malta" });
            await dbContext.Countries.AddAsync(new Country { Name = "Iceland" });
            await dbContext.Countries.AddAsync(new Country { Name = "Andorra" });
            await dbContext.Countries.AddAsync(new Country { Name = "Monaco" });
            await dbContext.Countries.AddAsync(new Country { Name = "Liechtenstein" });
            await dbContext.Countries.AddAsync(new Country { Name = "San Marino" });
            await dbContext.Countries.AddAsync(new Country { Name = "Holy See" });
        }
    }
}