using Microsoft.Extensions.Options;
using ProjectVitour.Services.CategoryServices;
using ProjectVitour.Settings;
using System.Reflection;
using Microsoft.AspNetCore.Localization;
using System.Globalization;
using ProjectVitour.Services.TourServices;
using ProjectVitour.Services.ReviewServices;
using ProjectVitour.Services.ReservationServices;
using ProjectVitour.Services.TourImageServices;
using ProjectVitour.Services.ContactMessageServices;
using ProjectVitour.Services.DestinationServices;
using ProjectVitour.Services.GuideServices;
using ProjectVitour.Services.EmailServices;
using ProjectVitour.Services.GeminiServices;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddLocalization(options => options.ResourcesPath = "Resources");
builder.Services.AddControllersWithViews()
    .AddViewLocalization()
    .AddDataAnnotationsLocalization();
builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddScoped<ITourService, TourService>();
builder.Services.AddScoped<IReviewService, ReviewService>();
builder.Services.AddScoped<IReservationService, ReservationService>();
builder.Services.AddScoped<ITourImageService, TourImageService>();
builder.Services.AddScoped<IContactMessageService, ContactMessageService>();
builder.Services.AddScoped<IDestinationService, DestinationService>();
builder.Services.AddScoped<IGuideService, GuideService>();
builder.Services.AddScoped<IEmailService, EmailService>();
builder.Services.AddHttpClient<IGeminiCostService, GeminiCostService>();
builder.Services.AddAutoMapper(Assembly.GetExecutingAssembly());

builder.Services.Configure<DatabaseSettings>(builder.Configuration.GetSection("DatabaseSettingKey"));


builder.Services.AddScoped<IDatabaseSettings>(sp =>
{
    return sp.GetRequiredService<IOptions<DatabaseSettings>>().Value;
});

var app = builder.Build();
var supportedCultures = new[] { "tr", "en", "de" };
var localizationOptions = new RequestLocalizationOptions()
    .SetDefaultCulture("tr")
    .AddSupportedCultures(supportedCultures)
    .AddSupportedUICultures(supportedCultures);

localizationOptions.RequestCultureProviders.Insert(0, new CookieRequestCultureProvider());
app.UseRequestLocalization(localizationOptions);

app.UseHttpsRedirection();
app.UseStaticFiles(); // .NET 10 öncesi için uyumluluk
app.MapStaticAssets();

app.UseRouting();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();

app.Run();