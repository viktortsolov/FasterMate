namespace FasterMate.Infrastrucutre.Data
{
    using System.Linq;

    using FasterMate.Infrastructure.Data;
    
    using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore;

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

        public DbSet<Comment> Comments { get; set; }

        public DbSet<PostLike> PostLikes { get; set; }

        public DbSet<ProfileFollower> ProfileFollowers { get; set; }

        public DbSet<Offer> Offers { get; set; }

        public DbSet<ProfileOffer> ProfileOffers { get; set; }

        public DbSet<Group> Groups { get; set; }

        public DbSet<GroupMember> GroupMembers { get; set; }

        public DbSet<Message> Messages { get; set; }


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

            builder.Entity<PostLike>()
               .HasKey(e => new { e.PostId, e.ProfileId });

            builder.Entity<ProfileOffer>()
                .HasKey(e => new { e.ProfileId, e.OfferId });

            builder.Entity<GroupMember>()
                .HasKey(e => new { e.GroupId, e.ProfileId });

            var entityTypes = builder
                .Model
                .GetEntityTypes()
                .ToList();

            var foreignKeys = entityTypes.
                SelectMany(e => e.GetForeignKeys().Where(f => f.DeleteBehavior == DeleteBehavior.Cascade));

            foreach (var foreignKey in foreignKeys)
            {
                foreignKey.DeleteBehavior = DeleteBehavior.Restrict;
            }
        }
    }
}