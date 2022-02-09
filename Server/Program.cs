using Microsoft.EntityFrameworkCore;
using StarRepo.Data;
using StarRepo.Data.Seed;
using StarRepo.Domain;
using StarRepo.Server;
using StarRepo.Server.GraphQL;

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
var db = System.IO.Path.Combine(dp.Database ?? string.Empty, "stardb.sqlite");
builder.Services.AddPooledDbContextFactory<StarContext>(opts =>
    opts.UseSqlite($"Data Source={db}"));
builder.Services.AddTransient<QueryRepo>();

builder.Services.AddGraphQLServer()
    .AddProjections()
    .AddFiltering()
    .AddSorting()
    .AddQueryType<Query>()
    .AddMutationType<Mutations>()
    .AddTypeExtension<ObservationTypeExtensions>();
    
builder.Services.AddRazorPages();

var app = builder.Build();

// seed the database - this is !!DEMO ONLY!! code
var provider = app.Services.CreateScope().ServiceProvider;
var factory = provider.GetService<IDbContextFactory<StarContext>>();
using (var ctx = factory!.CreateDbContext())
{
    if (await ctx!.Database.EnsureCreatedAsync())
    {
        var seeder = new Seeder(provider.GetService<ImageHandler>()!);
        await seeder.SeedAsync(ctx);
    }
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

app.MapGraphQL();

app.UseHttpsRedirection();

app.UseBlazorFrameworkFiles();
app.UseStaticFiles();

app.UseRouting();


app.MapRazorPages();
app.MapControllers();
app.MapFallbackToFile("index.html");

app.Run();
