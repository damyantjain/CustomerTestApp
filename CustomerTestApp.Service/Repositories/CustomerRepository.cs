using CustomerTestApp.Service.Exceptions;
using CustomerTestApp.Service.Models;

namespace CustomerTestApp.Service.Repositories
{
    public class CustomerRepository : ICustomerRepository
    {
        private readonly CustomerContext _context;

        public CustomerRepository(CustomerContext context)
        {
            _context = context;
        }

        public async IAsyncEnumerable<Models.Customer> GetAllCustomersAsync()
        {
            var enumerator = _context.Customers.AsAsyncEnumerable().GetAsyncEnumerator();

            while (true)
            {
                Models.Customer current;
                try
                {
                    if (!await enumerator.MoveNextAsync())
                    {
                        break;
                    }
                    current = enumerator.Current;
                }
                catch (Exception ex)
                {
                    throw new RepositoryException("An error occurred while retrieving customers.", ex);
                }

                yield return current;
            }

            await enumerator.DisposeAsync();
        }

        public async Task AddCustomerAsync(Models.Customer customer)
        {
            try
            {
                _context.Customers.Add(customer);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new RepositoryException("An error occurred while adding the customer.", ex);
            }
        }

        public async Task UpdateCustomerAsync(Models.Customer customer)
        {
            try
            {
                _context.Customers.Update(customer);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new RepositoryException("An error occurred while updating the customer.", ex);
            }
        }

        public async Task DeleteCustomerAsync(int id)
        {
            try
            {
                var customer = await _context.Customers.FindAsync(id);
                if (customer != null)
                {
                    _context.Customers.Remove(customer);
                    await _context.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                throw new RepositoryException("An error occurred while deleting the customer.", ex);
            }
        }
    }
}

