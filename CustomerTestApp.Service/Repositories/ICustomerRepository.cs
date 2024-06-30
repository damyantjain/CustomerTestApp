namespace CustomerTestApp.Service.Repositories
{
    public interface ICustomerRepository
    {
        /// <summary>
        /// Streams all customer from the database.
        /// </summary>
        /// <param name="filterType">Type of filter applied for getting data.</param>
        /// <param name="searchText">The text applied for the filter.</param>
        /// <returns></returns>
        IAsyncEnumerable<Models.Customer> GetFilteredCustomersAsync(FilterType filterType, string searchText, CancellationToken cancellationToken);

        /// <summary>
        /// Add a customer to the database.
        /// </summary>
        /// <param name="customer"></param>
        /// <returns></returns>
        Task AddCustomerAsync(Models.Customer customer);

        /// <summary>
        /// Update customer information in the database.
        /// </summary>
        /// <param name="customer"></param>
        /// <returns></returns>
        Task UpdateCustomerAsync(Models.Customer customer);

        /// <summary>
        /// Delete a customer from the database.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task DeleteCustomerAsync(Guid id);
    }
}
