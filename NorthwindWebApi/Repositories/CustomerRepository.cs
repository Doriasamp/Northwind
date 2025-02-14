using Microsoft.EntityFrameworkCore.ChangeTracking; // To use EntityEntry<T>
using Northwind.EntityModels;   // To use the Customer entity class
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal; // To use the ToArrayAsync() extension method
using Microsoft.Extensions.Caching.Hybrid;  // To use HybridCache class

namespace Northwind.WebApi.Repositories;

/// <summary>
/// The CustomerRepository class implements the ICustomerRepository interface
/// </summary>
public class CustomerRepository: ICustomerRepository
{
    private readonly HybridCache _cache;
    private readonly NorthwindContext _db;  // Use an instance data context field because it should not be cached due to the data context having internal caching.

    /// <summary>
    /// Constructor for the CustomerRepository class that accepts a data context and a cache
    /// </summary>
    /// <param name="db"></param>
    /// <param name="cache"></param>
    public CustomerRepository(NorthwindContext db, HybridCache cache)
    {
        _db = db;
        _cache = cache;
    }



    public async Task<Customer?> CreateAsync(Customer customer)
    {
        customer.CustomerId = customer.CustomerId.ToUpper();    // Normalize to uppercase

        // Add the customer to the database using EF Core.
        EntityEntry<Customer> addedCustomer = await _db.Customers.AddAsync(customer);
        int affected = await _db.SaveChangesAsync(); // Save changes to the database

        // The task result of SaveChangesAsync is the number of entities added to the database (should be 1)
        // If the customer was added to the database, then store in cache.
        if (affected == 1)
        {
            await _cache.SetAsync(customer.CustomerId, customer);
            return customer;
        }

        return null;
    }



    public Task<Customer[]> RetrieveAllAsync()
    {
        return _db.Customers.ToArrayAsync();
    }



    public async Task<Customer?> RetrieveAsync(string id, CancellationToken token = default)
    {
        id = id.ToUpper();
        return await _cache.GetOrCreateAsync(
            key: id, // unique key for the cache entry
            factory: async cancel => await _db.Customers.FirstOrDefaultAsync(c => c.CustomerId == id, token),
            cancellationToken: token
        );
    }



    public async Task<Customer?> UpdateAsync(Customer customer)
    {
        customer.CustomerId = customer.CustomerId.ToUpper();    // Normalize to uppercase

        _db.Customers.Update(customer);
        int affected = await _db.SaveChangesAsync(); // Save changes to the database

        // The task result of SaveChangesAsync is the number of entities added to the database (should be 1)
        // If the customer was added to the database, then store in cache.
        if (affected == 1)
        {
            await _cache.SetAsync(customer.CustomerId, customer);
            return customer;
        }
        return null;
    }



    public async Task<bool?> DeleteAsync(string id)
    {
        id = id.ToUpper();  // Normalize to uppercase
        Customer? c = await _db.Customers.FindAsync(id);

        if(c == null) return false;

        _db.Customers.Remove(c);
        
        int affected = await _db.SaveChangesAsync(); // Save changes to the actual database
        if (affected == 1)
        {
            await _cache.RemoveAsync(id);
            return true;
        }

        return null;
    }
}

