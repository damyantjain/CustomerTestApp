namespace CustomerTestApp.Service.Repositories
{
    public interface ICustomerRepository
    {
        /// <summary>
        /// Streams all customers from the database.
        /// </summary>
        /// <returns></returns>
        IAsyncEnumerable<Models.Customer> GetAllCustomersAsync();

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
        Task DeleteCustomerAsync(int id);
    }
}
