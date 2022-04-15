namespace FasterMate.Infrastructure.Seeding
{
    using FasterMate.Infrastructure.Data;
    using FasterMate.Infrastrucutre.Data;
 
    using Microsoft.AspNetCore.Identity;
    using Microsoft.Extensions.DependencyInjection;
    using System.Diagnostics.CodeAnalysis;

    [ExcludeFromCodeCoverage]
    public class AdminSeeder : ISeeder
    {
        public async Task SeedAsync(ApplicationDbContext dbContext, IServiceProvider serviceProvider)
        {
            var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();

            await dbContext.Profiles.AddAsync(new Profile
            {
                Id = "20097c1b-8949-457e-af41-42d6a25e2271",
                FirstName = "admin",
                LastName = "admin",
                Gender = Data.Enums.Gender.Male,
                BirthDate = DateTime.Now,
                CountryId = dbContext.Countries.FirstOrDefault().Id
            });

            var admin = new ApplicationUser()
            {
                UserName = "admin",
                Email = "admin@fastermate.com",
                ProfileId = "20097c1b-8949-457e-af41-42d6a25e2271"
            };

            var password = "admin1234";

            var adminResult = await userManager.CreateAsync(admin, password);

            if (adminResult.Succeeded)
            {
                await userManager.AddToRoleAsync(admin, "Administrator");
            }
        }
    }
}
