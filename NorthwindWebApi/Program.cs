using Northwind.EntityModels;   // To use AddNorthwindContext() extension method
using Microsoft.Extensions.Caching.Hybrid;
using Northwind.WebApi; // To use HybridCache class
using Northwind.WebApi.Repositories;  // To use ICustomerRepository interface

const string corsPolicyName = "allowWasmClient";

var builder = WebApplication.CreateBuilder(args);

// Add CORS (Cross-Origin Resource Sharing) services to the container, and congifure a policy to
// allow HTTP calls from clients with different porn numbers from the web service itself.
builder.Services.AddCors(options =>
    {
        options.AddPolicy(
            name: corsPolicyName,
            policy => { policy.WithOrigins("https://localhost:5152", "http://localhost:5151"); }
            );
    }
);

builder.Services.AddNorthwindContext(); // Register the Northwind database context class as a service
builder.Services.AddScoped<ICustomerRepository, CustomerRepository>(); // Register the CustomerRepository class as a service Web API

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi(documentName: "v2");

// Add HybridCache service to the container
builder.Services.AddHybridCache(options =>
{
    options.DefaultEntryOptions = new HybridCacheEntryOptions
    {
        Expiration = TimeSpan.FromSeconds(60), // Default overall expiration time for cache entries
        LocalCacheExpiration = TimeSpan.FromSeconds(30), // expiration time for local cache entries
    };
});


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseCors(corsPolicyName); // Use the CORS policy defined above

var summaries = new[]
{
    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
};

// MapGet is a shortcut for MapMethods("GET") call that registers a relative path of /weatherforecast to respond to HTTP GET requests
// The {days:int} is a route constraint that specifies that the days parameter must be an integer
// The ? makes the <days> parameter optional, and if missing it will default to 5
app.MapGet("/weatherforecast/{days:int:range(5, 21)?}",
        (int days = 5) => GetWeather(days)).WithName("GetWeatherForecast");

app.MapCustomers(); // Call the MapCustomers extension method to register the Customers API endpoints

app.Run();

