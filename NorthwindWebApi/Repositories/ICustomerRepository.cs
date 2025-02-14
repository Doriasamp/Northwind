using Northwind.EntityModels;   // To use the Customer entity class
namespace Northwind.WebApi.Repositories;

/// <summary>
/// Define an interface with five CRUD methods for the Customer entity
/// </summary>
public interface ICustomerRepository
{
    /// <summary>
    /// Create a new customer in the database and store it in the cache.
    /// </summary>
    /// <param name="customer"></param>
    /// <returns></returns>
    Task<Customer?> CreateAsync(Customer customer);


    /// <summary>
    /// Retrieve all customers from the database using ToArrayAsync() extension method.
    /// </summary>
    /// <returns></returns>
    Task<Customer[]> RetrieveAllAsync();


    /// <summary>
    /// Retrieve a customer by ID from the database using the cache if possible, <br/>
    /// or from the data model, and set it in the cache for future use.
    /// </summary>
    /// <param name="id"></param>
    /// <param name="token"></param>
    /// <returns></returns>
    Task<Customer?> RetrieveAsync(string id, CancellationToken token);


    /// <summary>
    /// Update a customer in the database and store it in the cache.
    /// </summary>
    /// <param name="customer"></param>
    /// <returns></returns>
    Task<Customer?> UpdateAsync(Customer customer);


    /// <summary>
    /// Delete a customer from the database, and if successful, remove the cached Customer from the cache.
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    Task<bool?> DeleteAsync(string id);
}

