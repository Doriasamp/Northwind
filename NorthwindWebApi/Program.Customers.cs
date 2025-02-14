using Microsoft.AspNetCore.Mvc; // To use ProblemDetails
using Northwind.EntityModels;   // To use the Customer entity class
using Northwind.WebApi.Repositories;  // To use ICustomerRepository interface

namespace Northwind.WebApi;

static partial class Program
{

    /// <summary>
    /// Define 5 minimal API endpoint route handlers that respond to HTTP GET requests for <br/>
    /// all customers, customers in a specified country, or a single customer by ID.
    /// </summary>
    /// <param name="app"></param>
    internal static void MapCustomers(this WebApplication app)
    {
        // ----------------- Customers API Endpoint -----------------
        // GET: /customers
        app.MapGet(
            pattern: "/customers",
            handler: async (ICustomerRepository repo) => { return await repo.RetrieveAllAsync();}
            );


        // ----------------- Customers API Endpoint -----------------
        // GET: customers/in[country]
        app.MapGet(
            pattern: "/customers/in/{country}",
            handler: async (ICustomerRepository repo, string country) =>
            {
                country = country.ToLower();
                return (await repo.RetrieveAllAsync()).Where(customer => customer?.Country?.ToLower() == country);
            });


        // ----------------- Customers API Endpoint -----------------
        // GET: customers/[id]
        app.MapGet(
            pattern: "/customers/{id:regex(^[A-Z]{{5}}$)}",
            handler: async Task<IResult> (string id, ICustomerRepository repo, CancellationToken token = default) =>
            {
                Customer? c = await repo.RetrieveAsync(id, token);
                if (c == null)
                {
                    return TypedResults.NotFound(); // 404 Resource not found.
                }

                return TypedResults.Ok(c);  // 200 OK with customer in body.
            }
        ).WithName("GetCustomer");


        // ----------------- Customers API Endpoint -----------------
        // POST: /customers
        // BODY: Customer (JSON)
        app.MapPost(
            pattern: "/customers",
            handler: async Task<IResult> (Customer c, ICustomerRepository repo) =>
            {
                if(c is null)
                {
                    return TypedResults.BadRequest(); // 400 Bad Request.
                }

                Customer? addedCustomer = await repo.CreateAsync(c);

                if(addedCustomer == null)
                {
                    return TypedResults.BadRequest("Repository failed to create customer.");
                }
                else
                {
                    return TypedResults.CreatedAtRoute(
                        routeName: "GetCustomer",
                        routeValues: new { id = addedCustomer.CustomerId.ToLower() },
                        value: addedCustomer
                    );
                }
            }
            );


        // ----------------- Customers API Endpoint -----------------
        // PUT: /customers/[id]
        // BODY: Customer (JSON)
        app.MapPut(
            pattern: "/customers/{id}",
            handler: async Task<IResult> (Customer c, string id, ICustomerRepository repo, CancellationToken token = default) =>
            {
                id = id.ToUpper();
                c.CustomerId = c.CustomerId.ToUpper();

                if (c is null || c.CustomerId != id)
                {
                    return TypedResults.BadRequest(); // 400 Bad Request.
                }

                Customer? existing = await repo.RetrieveAsync(id, token);

                if(existing == null)
                {
                    return TypedResults.NotFound(); // 404 Resource not found.
                }

                await repo.UpdateAsync(c);
                return TypedResults.NoContent(); // 204 No Content, success with no body.
            }
            );


        // ----------------- Customers API Endpoint -----------------
        // DELETE: /customers/[id]
        app.MapDelete(
            pattern: "/customers/{id}",
            handler: async Task<IResult> (string id, ICustomerRepository repo, CancellationToken token = default) =>
            {
                Customer? existing = await repo.RetrieveAsync(id, token);

                if (existing is null)
                {
                    return TypedResults.NotFound(); // 404 Resource not found.
                }

                bool? deleted = await repo.DeleteAsync(id);

                if (deleted.HasValue && deleted.Value) // short-circuit AND
                {
                    return TypedResults.NoContent(); // 204 No Content, success with no body.
                }
                else
                {
                    return TypedResults.BadRequest($"Customer {id} was found, but failed to delete."); // 400 Bad Request.
                }
            }
            );

    }

}