using Microsoft.EntityFrameworkCore;
using StarRepo.Data;
using StarRepo.Data.Seed;
using StarRepo.Server;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var dp = new DataPaths();
builder.Configuration.Bind(nameof(DataPaths), dp);

if (!string.IsNullOrEmpty(dp.Database) 
    && dp.CreateIfNotExists 
    && !Directory.Exists(dp.Database))
{
    Directory.CreateDirectory(dp.Database);
}

builder.Services.AddScoped<ImageHandler>(
    sp => new ImageHandler(dp.Images ?? string.Empty, dp.CreateIfNotExists));
builder.Services.AddScoped<Seeder>();
var db = Path.Combine(dp.Database ?? string.Empty, "stardb.sqlite");
builder.Services.AddDbContextFactory<StarContext>(opts =>
    opts.UseSqlite($"Data Source={db}"));

builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();

var app = builder.Build();

// seed the database - this is !!DEMO ONLY!! code
using (var ctx = app.Services.CreateScope().ServiceProvider.GetService<StarContext>())
{
    await ctx!.CheckAndSeedAsync();
}

    // Configure the HTTP request pipeline.
    if (app.Environment.IsDevelopment())
    {
        app.UseWebAssemblyDebugging();
    }
    else
    {
        app.UseExceptionHandler("/Error");
        // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
        app.UseHsts();
    }

app.UseHttpsRedirection();

app.UseBlazorFrameworkFiles();
app.UseStaticFiles();

app.UseRouting();


app.MapRazorPages();
app.MapControllers();
app.MapFallbackToFile("index.html");

app.Run();
