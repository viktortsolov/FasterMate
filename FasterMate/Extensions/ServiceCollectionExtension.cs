namespace FasterMate.Extensions
{
    using FasterMate.Core.Contracts;
    using FasterMate.Core.Services;
    using FasterMate.Infrastructure.Common;
    using FasterMate.Infrastrucutre.Data;

    using Microsoft.EntityFrameworkCore;

    public static class ServiceCollectionExtension
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddScoped(typeof(IRepository<>), typeof(Repository<>));

            services.AddScoped<IProfileService, ProfileService>()
                    .AddScoped<ICountryService, CountryService>()
                    .AddScoped<IUserService, UserService>()
                    .AddScoped<IImageService, ImageService>()
                    .AddScoped<IPostService, PostService>()
                    .AddScoped<ICommentService, CommentService>()
                    .AddScoped<IOfferService, OfferService>()
                    .AddScoped<IGroupService, GroupService>()
                    .AddScoped<IMessageService, MessageService>();

            return services;
        }

        public static IServiceCollection AddApplicationDbContexts(this IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("DefaultConnection");
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(connectionString));

            services.AddDatabaseDeveloperPageExceptionFilter();

            return services;
        }
    }
}
