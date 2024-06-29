using CustomerTestApp.Service.Exceptions;
using CustomerTestApp.Service.Models;
using Microsoft.EntityFrameworkCore;
using System.Runtime.CompilerServices;

namespace CustomerTestApp.Service.Repositories
{
    /// <summary>
    /// The Customer Repository class is responsible for handling the customer data with the database.
    /// </summary>
    public class CustomerRepository : ICustomerRepository
    {
        private readonly CustomerContext _context;

        private readonly object _lock = new object();

        public CustomerRepository(CustomerContext context)
        {
            _context = context;
        }

        private IQueryable<Customer> GetFilteredQuery(FilterType filterType, string searchText)
        {
            var query = _context.Customers.AsQueryable();

            if (!string.IsNullOrWhiteSpace(searchText))
            {
                switch (filterType)
                {
                    case FilterType.Name:
                        query = query.Where(x => (x.FirstName + " " + x.LastName).ToLower().Contains(searchText) ||
                                                 x.FirstName.ToLower().Contains(searchText) ||
                                                 x.LastName.ToLower().Contains(searchText));
                        break;
                    case FilterType.Email:
                        query = query.Where(x => x.Email.ToLower().Contains(searchText));
                        break;
                    case FilterType.All:
                        query = query.Where(x => (x.FirstName + " " + x.LastName).ToLower().Contains(searchText) ||
                                                 x.FirstName.ToLower().Contains(searchText) ||
                                                 x.LastName.ToLower().Contains(searchText) ||
                                                 x.Email.ToLower().Contains(searchText));
                        break;
                }
            }

            return query;
        }

        public async IAsyncEnumerable<Customer> GetFilteredCustomersAsync(FilterType filterType, string searchText, [EnumeratorCancellation] CancellationToken cancellationToken)
        {
            var query = _context.Customers.AsQueryable();

            if (!string.IsNullOrWhiteSpace(searchText))
            {
                searchText = searchText.Trim().ToLower();
                query = GetFilteredQuery(filterType, searchText);
            }

            var enumerator = query.AsAsyncEnumerable().GetAsyncEnumerator(cancellationToken);

            while (true)
            {
                Customer current;
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
                    throw new RepositoryException("An error occurred while retrieving filtered customers.", ex);
                }

                yield return current;
            }

            await enumerator.DisposeAsync();
        }


        public async Task AddCustomerAsync(Customer customer)
        {
            try
            {
                lock(_lock)
                {
                    _context.Customers.Add(customer);
                }
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new RepositoryException("An error occurred while adding the customer.", ex);
            }
        }

        public async Task UpdateCustomerAsync(Customer customer)
        {
            try
            {
                lock(_lock)
                {
                    _context.Customers.Update(customer);
                }
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
                lock (_lock)
                {
                    var customer = _context.Customers.Find(id);
                    if (customer != null)
                    {
                        _context.Customers.Remove(customer);
                    }
                }
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new RepositoryException("An error occurred while deleting the customer.", ex);
            }
        }
    }
}

