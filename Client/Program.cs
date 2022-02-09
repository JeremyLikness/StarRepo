using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using StarRepo.Client;
using StarRepo.Client.GraphQL;
using StarRepo.Client.Shared;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");
var baseAddress = new Uri(builder.HostEnvironment.BaseAddress);
var baseAddressGql = new Uri(baseAddress, "graphql");
var uriBuilder = new UriBuilder(baseAddressGql);
uriBuilder.Scheme = "wss";
var baseAddressWs = uriBuilder.Uri;

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = baseAddress });
builder.Services.AddStarClient() 
    .ConfigureHttpClient(client => client.BaseAddress = baseAddressGql)
    .ConfigureWebSocketClient(client => client.Uri = baseAddressWs);
builder.Services.AddScoped<StarClientService>();
await builder.Build().RunAsync();
