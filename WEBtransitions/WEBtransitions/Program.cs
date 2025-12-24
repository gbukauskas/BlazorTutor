//using Cloudcrate.AspNetCore.Blazor.Browser.Storage;
//using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;
using WEBtransitions.ClassLibraryDatabase.DBContext;
using WEBtransitions.Components;
using WEBtransitions.Services;
using WEBtransitions.Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<CookieService>();


/* DbContext is possible but dangerous. Use AddDbContext in single-thread environment, use DbContextFactory in all other cases.
builder.Services.AddDbContext<NorthwindContext>(options =>
        {
            options.UseSqlite(
                builder.Configuration.GetConnectionString("DefaultConnection"));
        });

// options.UseSqlite(
// options.UseSqlServer(
// options.UseMySql(
// options.UseNpgsql(

*/


builder.Services.AddDbContextFactory<NorthwindContext>(options =>
{
    options.UseSqlite(
        builder.Configuration.GetConnectionString("DefaultConnection"));
});

builder.Services
    .AddScoped<CategorySvc>()
    .AddScoped<CustomerSvc>()
    .AddScoped<EmployeeSvc>()
    .AddScoped<RegionSvc>()
    .AddScoped<TerritorySvc>()
    .AddScoped<IKeyGenerator, KeyGenerator>();

builder.Services.AddSingleton<StateSvc, StateSvc>();

//builder.Services.AddSingleton<IStateData, StateData>();
//builder.Services.AddScoped<IStateData, StateData>();

const string UserID = "{69FB454F-B49B-4876-A0CD-AE727DF941C1}"; // For DEMO only. Real application must put here value from authentication.
string ignoreConcurrency = builder.Configuration.GetValue<string?>("AppSettings:IgnoreConcurrency") ?? "hidden";
builder.Services.AddCascadingValue("StateKey", sp => new AppStateKey("WEBtransitions", UserID));
builder.Services.AddCascadingValue("IgnoreConcurrency", sp => ignoreConcurrency);

int cookieExpires = builder.Configuration.GetValue<int?>("AppSettings:CookieExpires") ?? 1;
builder.Services.AddCascadingValue("CookieDuration", sp => cookieExpires);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
}


app.UseAntiforgery();

app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();

// Change SQL drive:
// https://www.bing.com/videos/riverview/relatedvideo?q=sql+move+database+to+another+drive&mid=358CF6A17A7AA5E9760F358CF6A17A7AA5E9760F&FORM=VIRE
// https://www.learnblazor.com/layouts
// Free hosting https://www.monsterasp.net/#plans
// https://learn.microsoft.com/en-us/aspnet/core/blazor/state-management/protected-browser-storage?view=aspnetcore-9.0
// https://learn.microsoft.com/en-us/ef/ef6/fundamentals/connection-management          // Unlock SQLite