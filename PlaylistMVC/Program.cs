using FluentAssertions.Common;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.EntityFrameworkCore;
using PlaylistMVC.Models;
using Microsoft.AspNetCore.Identity;
using PlaylistMVC.Data;
using PlaylistMVC.Areas.Identity.Data;

var builder = WebApplication.CreateBuilder(args);

//Adding database context

builder.Services.AddDbContext<MyDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddDbContext<AuthDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("AuthDbContextConnection")));
builder.Services.AddDefaultIdentity<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = false)
    .AddEntityFrameworkStores<AuthDbContext>();


// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddRazorPages();

builder.Services.Configure<IdentityOptions>(options =>
{
    options.Password.RequireUppercase = false;
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();


//using authentication
app.UseAuthentication();

app.UseAuthorization();



app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapRazorPages();

using (var serviceScope = app.Services.GetRequiredService<IServiceScopeFactory>().CreateScope())
{

    var context = serviceScope.ServiceProvider.GetRequiredService<MyDbContext>();
    try
    {
        var genres = context.Genres;
        if (!genres.Any())
        {
            List<Genre> genresList = new List<Genre>();
            genresList.Add(new Genre { Name = "Pop" });
            genresList.Add(new Genre { Name = "Rock" });
            genresList.Add(new Genre { Name = "Metal" });
            genresList.Add(new Genre { Name = "Hip Hop" });
            genresList.Add(new Genre { Name = "Country Music" });
            genresList.Add(new Genre { Name = "Jazz" });
            genresList.Add(new Genre { Name = "Disco" });

            genres.AddRange(genresList);
            context.SaveChanges();

        }
    }
    catch (Exception exception)
    {
        Console.WriteLine(exception.Message);
        throw;
    }
}

app.Run();

