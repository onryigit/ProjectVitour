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

var builder = WebApplication.CreateBuilder(args);

// --- LOCALIZATION (ÇOKLU DİL) AYARLARI ---
builder.Services.AddLocalization(options => options.ResourcesPath = "Resources");
builder.Services.AddControllersWithViews()
    .AddViewLocalization()
    .AddDataAnnotationsLocalization();
// ------------------------------------------

// Add services to the container.
builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddScoped<ITourService, TourService>();
builder.Services.AddScoped<IReviewService, ReviewService>();
builder.Services.AddScoped<IReservationService, ReservationService>();
builder.Services.AddScoped<ITourImageService, TourImageService>();
builder.Services.AddScoped<IContactMessageService, ContactMessageService>();
builder.Services.AddScoped<IDestinationService, DestinationService>();
builder.Services.AddScoped<IGuideService, GuideService>();
builder.Services.AddAutoMapper(Assembly.GetExecutingAssembly());

builder.Services.Configure<DatabaseSettings>(builder.Configuration.GetSection("DatabaseSettingKey"));


builder.Services.AddScoped<IDatabaseSettings>(sp =>
{
    return sp.GetRequiredService<IOptions<DatabaseSettings>>().Value;
});

// builder.Services.AddControllersWithViews(); // Yukarıya taşıdık

var app = builder.Build();

// --- LOCALIZATION MIDDLEWARE ---
var supportedCultures = new[] { "tr", "en", "de" };
var localizationOptions = new RequestLocalizationOptions()
    .SetDefaultCulture("tr")
    .AddSupportedCultures(supportedCultures)
    .AddSupportedUICultures(supportedCultures);

localizationOptions.RequestCultureProviders.Insert(0, new CookieRequestCultureProvider());
app.UseRequestLocalization(localizationOptions);
// --------------------------------

app.UseHttpsRedirection();
app.UseStaticFiles(); // .NET 10 öncesi için uyumluluk
app.MapStaticAssets();

app.UseRouting();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();

// --- BİR KEREYE MAHSUS VERİTABANI SEED (TEST VERİSİ) İŞLEMİ ---
//using (var scope = app.Services.CreateScope())
//{
//    var dbSettings = scope.ServiceProvider.GetRequiredService<IDatabaseSettings>();
//    ProjectVitour.DataSeeder.ClearAndSeed(dbSettings);
//}
// -------------------------------------------------------------

app.Run();