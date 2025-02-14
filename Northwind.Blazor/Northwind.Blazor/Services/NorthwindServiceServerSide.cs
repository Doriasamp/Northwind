using Microsoft.EntityFrameworkCore;    // To use ToListAsync<T> extension method
namespace Northwind.Blazor.Services;

public class NorthwindServiceServerSide : INorthwindService
{
    // DbContext instance
    private readonly NorthwindContext _dbContext;

    // Constructor
    public NorthwindServiceServerSide(NorthwindContext dbContext)
    {
        _dbContext = dbContext;
    }

    // Overriden methods from INorthwindService
    public Task<List<Customer>> GetCustomersAsync()
    {
        return _dbContext.Customers.ToListAsync();
    }

    public Task<List<Customer>> GetCustomersAsync(string country)
    {
        return _dbContext.Customers.Where(c => c.Country == country).ToListAsync();
    }

    public Task<Customer?> GetCustomerAsync(string id)
    {
        return _dbContext.Customers.FirstOrDefaultAsync(c => c.CustomerId == id);
    }

    public Task<Customer> CreateCustomerAsync(Customer c)
    {
        _dbContext.Customers.Add(c);
        _dbContext.SaveChangesAsync();
        return Task.FromResult(c);
    }

    public Task<Customer> UpdateCustomerAsync(Customer customer)
    {
        _dbContext.Entry(customer).State = EntityState.Modified;
        _dbContext.SaveChangesAsync();
        return Task.FromResult(customer);
    }

    public Task DeleteCustomerAsync(string id)
    {
        Customer? customer = _dbContext.Customers.FirstOrDefaultAsync(c => c.CustomerId == id).Result;

        if (customer == null)
        {
            return Task.CompletedTask;
        }
        else
        {
            _dbContext.Customers.Remove(customer);
            return _dbContext.SaveChangesAsync();
        }
    }
}

