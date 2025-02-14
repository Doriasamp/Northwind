using Northwind.Blazor.Client.Pages;
using Northwind.Blazor.Components;
using Northwind.Blazor.Services;    // To use INorthwindService.

var builder = WebApplication.CreateBuilder(args);

// Register the Northwind database context in the dependency services collection
builder.Services.AddNorthwindContext(relativePath: @"..\..");

// Register the Northwind service server-side as a transient service that implements the INorthwindService interface
builder.Services.AddTransient<INorthwindService, NorthwindServiceServerSide>();


// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents()
    .AddInteractiveWebAssemblyComponents();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseWebAssemblyDebugging();
}
else
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();


app.UseAntiforgery();

app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode()
    .AddInteractiveWebAssemblyRenderMode()
    .AddAdditionalAssemblies(typeof(Northwind.Blazor.Client._Imports).Assembly);

app.Run();
