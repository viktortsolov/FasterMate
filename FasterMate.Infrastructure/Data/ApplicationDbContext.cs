﻿namespace FasterMate.Infrastrucutre.Data
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

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<ApplicationUser>()
                .Property(p => p.ProfileId)
                .IsRequired(true);

            builder.Entity<Profile>()
                .Property(p => p.ImageId)
                .IsRequired(false);

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