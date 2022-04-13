namespace FasterMate.Test
{
    using FasterMate.Infrastrucutre.Data;
    using Microsoft.Data.Sqlite;
    using Microsoft.EntityFrameworkCore;

    public class InMemoryDbContext
    {
        private readonly SqliteConnection connection;
        private readonly DbContextOptions<ApplicationDbContext> dbContextOpt;

        public InMemoryDbContext()
        {
            connection = new SqliteConnection("Filename=:memory:");
            connection.Open();

            dbContextOpt = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseSqlite(connection)
                .Options;

            using var context = new ApplicationDbContext(dbContextOpt);

            context.Database.EnsureCreated();
        }

        public ApplicationDbContext CreateContext() => new ApplicationDbContext(dbContextOpt)
;
        public void Dispose() => connection.Dispose();
    }
}
