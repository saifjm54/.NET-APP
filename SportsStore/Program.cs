using Microsoft.EntityFrameworkCore;
using SportsStore.Models;
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<StoreDbContext>(opts =>
{
    opts.UseNpgsql(
    builder.Configuration["ConnectionStrings:SportsStoreConnection"]);
});
builder.Services.AddScoped<IStoreRepository, EFStoreRepository>();
builder.Services.AddScoped<IOrderRepository, EFOrderRepository>();
builder.Services.AddRazorPages();
// Enabling sessions
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession();
builder.Services.AddScoped<Cart>(sp => SessionCart.GetCart(sp));
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>(); // the same object should always used ,So u can access the current session in the SessionCart class.
builder.Services.AddServerSideBlazor();
var app = builder.Build();

//app.MapGet("/", () => "Hello World!");

app.UseStaticFiles();
app.UseSession();
app.MapControllerRoute("catpage", "{category}/Page{productPage:int}", // Lists the first page of products from all categories
new { Controller = "Home", action = "Index" }
);
app.MapControllerRoute("page", "Page{productPage:int}",
new { Controller = "Home", action = "Index", productPage = 1 }); // Lists the specified page , showing items from all categories

app.MapControllerRoute("category", "{category}", new { Controller = "Home", action = "Index", productPage = 1 }); //Shows the first page of items from a specific category
app.MapControllerRoute("pagination",
"Products/Page{productPage}",
new { Controller = "Home", action = "Index", productPage = 1 }); // Shows the specified page of items from the specified category 
app.MapDefaultControllerRoute();
app.MapRazorPages();
app.MapBlazorHub();
app.MapFallbackToPage("/admin/{*catchall}", "/Admin/Index");
SeedData.EnsurePopulated(app);

app.Run();
