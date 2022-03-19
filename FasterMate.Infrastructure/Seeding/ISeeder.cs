namespace FasterMate.Infrastructure.Seeding
{
    using FasterMate.Infrastrucutre.Data;

    public interface ISeeder
    {
        Task SeedAsync(ApplicationDbContext dbContext, IServiceProvider serviceProvider);
    }
}
