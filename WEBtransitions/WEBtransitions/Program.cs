using Microsoft.EntityFrameworkCore;
using WEBtransitions.Components;
using WEBtransitions.ClassLibraryDatabase.DBContext;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

/* It is possible but dangerous. Use AddDbContext in single-thread environment, use DbContextFactory in all other cases.
builder.Services.AddDbContext<NorthwindContext>(options =>
        {
            options.UseSqlite(
                builder.Configuration.GetConnectionString("DefaultConnection"));
        });
*/
builder.Services.AddDbContextFactory<NorthwindContext>(options =>
{
    options.UseSqlite(
        builder.Configuration.GetConnectionString("DefaultConnection"));
});

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