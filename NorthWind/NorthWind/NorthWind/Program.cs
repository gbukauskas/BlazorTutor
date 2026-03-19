using ClassLibraryDatabase.DB_Context;
using ClassLibraryDatabase.DB_Context.Models;
using Microsoft.EntityFrameworkCore;
using NorthWind.Components;
using NorthWind.Services;
using NorthWind.Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();
//.AddInteractiveWebAssemblyComponents();

builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<CookieService>();


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


const string UserID = "{69FB454F-B49B-4876-A0CD-AE727DF941C1}"; // For DEMO only. Real application must put here value from authentication.
string ignoreConcurrency = builder.Configuration.GetValue<string?>("AppSettings:IgnoreConcurrency") ?? "hidden";
builder.Services.AddCascadingValue("StateKey", sp => new AppStateKey("WEBtransitions", UserID));
builder.Services.AddCascadingValue("IgnoreConcurrency", sp => ignoreConcurrency);

int cookieExpires = builder.Configuration.GetValue<int?>("AppSettings:CookieExpires") ?? 1;
builder.Services.AddCascadingValue("CookieDuration", sp => cookieExpires);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    //app.UseWebAssemblyDebugging();
}
else
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
}
app.UseStatusCodePagesWithReExecute("/not-found", createScopeForStatusCodePages: true);
app.UseAntiforgery();

app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();
    //.AddInteractiveWebAssemblyRenderMode()
    //.AddAdditionalAssemblies(typeof(NorthWind.Client._Imports).Assembly);

app.Run();
