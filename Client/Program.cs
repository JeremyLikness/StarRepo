using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using StarRepo.Client;
using StarRepo.Client.GraphQL;
using StarRepo.Client.Shared;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");
builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
builder.Services.AddStarClient() 
    .ConfigureHttpClient(client => client.BaseAddress = new Uri($"{builder.HostEnvironment.BaseAddress}graphql"))
    .ConfigureWebSocketClient(client => client.Uri = new Uri($"{builder.HostEnvironment.BaseAddress}graphql"));
builder.Services.AddScoped<StarClientService>();
await builder.Build().RunAsync();
