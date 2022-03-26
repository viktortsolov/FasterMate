namespace FasterMate.Infrastrucutre.Data
{
    using FasterMate.Infrastructure.Data;
    using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore;
    using System.Linq;

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Profile> Profiles { get; set; }

        public DbSet<Image> Images { get; set; }

        public DbSet<Country> Countries { get; set; }

        public DbSet<Post> Posts { get; set; }

        public DbSet<ProfileFollower> ProfileFollowers { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<ApplicationUser>()
                .Property(p => p.ProfileId)
                .IsRequired(true);

            builder.Entity<Profile>()
                .Property(p => p.ImageId)
                .IsRequired(false);

            builder.Entity<ProfileFollower>()
                .HasKey(e => new { e.ProfileId, e.FollowerId });

            builder.Entity<Profile>()
                .HasMany(p => p.Followers)
                .WithOne(x => x.Follower);

            builder.Entity<Profile>()
                .HasMany(p => p.Following)
                .WithOne(x => x.Profile);

            var entityTypes = builder
                .Model
                .GetEntityTypes()
                .ToList();

            //Disable cascade delete
            var foreignKeys = entityTypes.
                SelectMany(e => e.GetForeignKeys().Where(f => f.DeleteBehavior == DeleteBehavior.Cascade));

            foreach (var foreignKey in foreignKeys)
            {
                foreignKey.DeleteBehavior = DeleteBehavior.Restrict;
            }
        }
    }
}