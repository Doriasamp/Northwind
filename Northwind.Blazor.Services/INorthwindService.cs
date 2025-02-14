/*
 * Define a contract for a local service that abstracts CRUD operations for the Customer entity.
 */

using Northwind.EntityModels;   // To use Customer entity

namespace Northwind.Blazor.Services
{
    public interface INorthwindService
    {
        Task<List<Customer>> GetCustomersAsync();
        Task<List<Customer>> GetCustomersAsync(string country);
        Task<Customer?> GetCustomerAsync(string id);
        Task<Customer> CreateCustomerAsync(Customer c);
        Task<Customer> UpdateCustomerAsync(Customer c);
        Task DeleteCustomerAsync(string id);
    }
}
