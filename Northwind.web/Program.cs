using Northwind.web.Components; // To use App component
using Northwind.EntityModels;   // To use AddNorthwindContext extension method

// Creates a host for the website using defaults for a web host
// that is then built up with the configuration and services needed for the website.
#region Configure the web server host and services
var builder = WebApplication.CreateBuilder(args);
builder.Services.AddRazorComponents();  // Add Razor Components services
builder.Services.AddNorthwindContext(); // Add Northwind database context services
var app = builder.Build();
#endregion

#region Configure the HTTP pipeline and routes for automatically redirects HTTP requests to HTTPS
if (!app.Environment.IsDevelopment())
{
    app.UseHsts();
}
else
{
    app.UseHttpsRedirection();
}
app.UseAntiforgery();


app.UseDefaultFiles();  // Enable default files (index.html, default.html, etc.)
app.MapStaticAssets();  // Enable static files (html, css, js, images, etc.) .NET 9 or later
//app.UseStaticFiles();  // Enable static files (html, css, js, images, etc.) .NET8 or earlier

app.MapRazorComponents<App>();  // Map the Razor component to the root URL

app.MapGet("/env", () => $"Environment is {app.Environment.EnvironmentName}");
#endregion



// Start the web server, host the website, and wait for requests.
app.Run();  // Thread-blocking call to start the web server and wait for requests
WriteLine("This executes after the web server has stopped!");
