using FasterMate.Core.Constants;
using FasterMate.Core.Contracts;
using FasterMate.Core.Services;
using FasterMate.Infrastructure.Common;
using FasterMate.Infrastructure.Data;
using FasterMate.Infrastrucutre.Data;
using FasterMate.ModelBinders;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));

builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder
    .Services
    .AddScoped(typeof(IRepository<>), typeof(Repository<>));

builder
    .Services
    .AddTransient<IProfileService, ProfileService>()
    .AddTransient<ICountryService, CountryService>();

builder
    .Services.AddDefaultIdentity<ApplicationUser>(options =>
    {
        options.Password.RequireDigit = false;
        options.Password.RequireLowercase = false;
        options.Password.RequireNonAlphanumeric = false;
        options.Password.RequireUppercase = false;
    })
    .AddEntityFrameworkStores<ApplicationDbContext>();

builder
    .Services.AddControllersWithViews()
    .AddMvcOptions(options =>
    {
        options.ModelBinderProviders.Insert(0, new DecimalModelBinderProvider());
        options.ModelBinderProviders.Insert(1, new DoubleModelBinderProvider());
        options.ModelBinderProviders.Insert(2, new DateTimeModelBinderProvider(FormatingConstant.NormalDateFormat));
    });

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection()
    .UseStaticFiles()
    .UseRouting()
    .UseAuthentication()
    .UseAuthorization()
    .UseEndpoints(endpoints =>
    {
        endpoints.MapDefaultControllerRoute();
        endpoints.MapRazorPages();
    });
app.UseAuthentication();
app.Run();