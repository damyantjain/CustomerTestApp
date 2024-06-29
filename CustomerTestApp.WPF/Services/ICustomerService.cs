using CustomerTestApp.Service;

namespace CustomerTestApp.WPF.Services
{
    /// <summary>
    /// The ICustomerService interface defines the service methods for the customer.
    /// </summary>
    public interface ICustomerService
    {
        /// <summary>
        /// This returns all customers from the database.
        /// </summary>
        /// <returns>returns all customers from the database</returns>
        IAsyncEnumerable<CustomerModel> GetAllCustomersAsync(FilterType filterType, string searchText, CancellationToken cancellationToken);

        /// <summary>
        /// This adds a customer to the database.
        /// </summary>
        /// <param name="customer">customer object to be added</param>
        /// <returns></returns>
        Task<CustomerResponse> AddCustomerAsync(CustomerModel customer);

        /// <summary>
        /// This updates customer information in the database.
        /// </summary>
        /// <param name="customer">customer object to be updated</param>
        /// <returns></returns>
        Task<CustomerResponse> UpdateCustomerAsync(CustomerModel customer);

        /// <summary>
        /// This deletes a customer from the database.
        /// </summary>
        /// <param name="customerId">Id of the customer to be deleted.</param>
        /// <returns></returns>
        Task<CustomerResponse> DeleteCustomerAsync(int customerId);
    }
}
