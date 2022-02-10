# StarRepo

This repository demonstrates how to use GraphQL in an ASP.NET Core server with a .NET client (in this example, Blazor WASM). It uses [HotChocolate](https://chillicream.com/docs/hotchocolate) and [StrawberryShake](https://chillicream.com/docs/strawberryshake).

I built this example for the [Tulsa .NET User Group](https://www.meetup.com/TulsaDevelopers-net/events/283643087/).

It includes examples of queries, type extensions, resolvers, mutations, and subscriptions.

> 💡 __TIP__ The project is only partially implemented. You can expand it as a learning exercise by adding these missing features: delete capabilities, "add telescope", target maintenance, and adding new observations.

## Quickstart

1. Clone the repo: `git clone https://github.com/JeremyLikness/StarRepo`
2. Compile and run

The app is built to automatically create and seed the data.

> ⚠️ __WARNING__ The app "as is" uses a Windows-specific API to generate thumbnails. You can see a compiler supression comment that wraps use of `System.Common.Drawing` that is not supported on other platforms. You can remove the code and reference to compile and run on other platforms.

Thanks,

[@JeremyLikness](https://twitter.com/JeremyLikness)


