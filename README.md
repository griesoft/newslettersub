# Newsletter Subscribing Service
A very simple newsletter subscription service for ASP.NET Core, which provides functionality to manage and maintain a collection of contacts and newsletter subscribers.

The main purpose of developing this was the need to implement this newsletter subscribing feature in many different projects. So because this was developed mostly for my own usage and without bigger visions in mind, the development of this service might be poor in the future.

[![Build Status](https://dev.azure.com/griesingersoftware/Newsletter%20Sub%20Service/_apis/build/status/NewsletterSub%20CI%20Pipeline?branchName=master)](https://dev.azure.com/griesingersoftware/Newsletter%20Sub%20Service/_build/latest?definitionId=16&branchName=master)
[![NuGet](https://badgen.net/nuget/v/NewsletterSub)](https://www.nuget.org/packages/NewsletterSub)
[![GitHub Release](https://badgen.net/github/release/jgdevlabs/newslettersub)](https://github.com/jgdevlabs/newslettersub/releases)

## Installation

Install via [NuGet](https://www.nuget.org/packages/NewsletterSub/) using:

``PM> Install-Package NewsletterSub``

## Setup

Before you can use the service you need to configure the database context for the service. We assumes that you have already an existing **Entity Framweork Core** `DbContext` setup. 
If not, follow the [Microsoft docs](https://docs.microsoft.com/en-us/ef/core/get-started/?tabs=netcore-cli) on how to do so.
Of course you may also create an own new DbContext for the service, it's up to you.

Now in your existing DbContext, implement the `INewsletterSubDbContext` interface. Don't forgt to add a reference to the `NewsletterSub.Data` namespace.

You have now implemented two new DbSet's and your DbContext should look something like this

```csharp
public class MyDbContext : DbContext, INewsletterSubDbContext
{
    public DbSet<Contact> Contacts { get; set; }
    public DbSet<NewsletterSubscription> NewsletterSubscriptions { get; set; }
}
```

Now override the `OnModelCreating(ModelBuilder modelBuilder)` method of your DbContext and copy & paste this line of code `modelBuilder.BuildNewsletterSubModels()` in it. So that would look something like this:

```csharp
protected override void OnModelCreating(ModelBuilder modelBuilder)
{
    // Any existing code...

    modelBuilder.BuildNewsletterSubModels();

    // Any existing code...
}
```

The only steps left now is to create a migration and update your database. Just run the following commands on your `.NET Core CLI`

    dotnet ef migrations add <Migration Name>

**AND**

    dotnet ef database update

No additional code setup is required other then the basic service registration in your *Startup.cs*. Just add the following line of code in the `ConfigureServices(IServiceCollection services)` method, anywhere after registering your DbContext.

```csharp
services.AddNewsletterService();
```

## Usage

You can request an instance of the service with `Dependency Injection (DI)` by just adding the `INewsletterService` as a parameter to your constructor of your controller.

```csharp
public class MyWebController : Controller
{
    private readonly INewsletterService _newsletterService;
    
    public MyWebController(INewsletterService newsletterService)
    {
        _newsletterService = newsletterService;    
    }
}
```

Now you can just use the service to add or remove newsletter subscribers.

#### Add subscriber

```csharp
await _newsletterService.AddNewsletterSubscription("me@demo.com, "Product X");
```

#### Remove subscriber


```csharp
await _newsletterService.RemoveNewsletterSubscription("me@demo.com, "Product X");
```
