namespace FasterMate.Api.Extensions
{
    using FasterMate.Core.Contracts;
    using FasterMate.Core.Services;
    using FasterMate.Infrastructure.Common;
    using FasterMate.Infrastrucutre.Data;

    using Microsoft.EntityFrameworkCore;

    public static class ApiServiceCollectionExtensions
    {
        public static IServiceCollection AddApiServices(this IServiceCollection services)
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

        public static IServiceCollection AddApiDbContexts(this IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("DefaultConnection");
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(connectionString));

            return services;
        }
    }
}
