using CustomerTestApp.Service.Exceptions;
using CustomerTestApp.Service.Repositories;
using Grpc.Core;
using CustomerTestApp.Service.Models;

namespace CustomerTestApp.Service.Services
{
    /// <summary>
    /// The Customer Service class provides the gRPC service implementation for the customers.
    /// </summary>
    public class CustomerService : CustomerManagement.CustomerManagementBase
    {
        private readonly ICustomerRepository _customerRepository;

        /// <summary>
        /// The customer repository is injected here.
        /// </summary>
        /// <param name="context"></param>
        public CustomerService(ICustomerRepository customerRepository)
        {
            _customerRepository = customerRepository;
        }

        /// <summary>
        /// This method returns all customers from the database.
        /// </summary>
        /// <param name="request">The filter applied for getting the customers.</param>
        /// <param name="responseStream">This is the response stream having the customer object</param>
        /// <param name="context">This is the server call context</param>
        /// <returns></returns>
        public override async Task GetAllCustomers(CustomerFilter request, IServerStreamWriter<CustomerModel> responseStream, ServerCallContext context)
        {
            try
            {
                var cancellationToken = context.CancellationToken;
                await foreach (var customer in _customerRepository.GetFilteredCustomersAsync(request.FilterType, request.SearchText, cancellationToken))
                {
                    var customerMessage = new CustomerModel
                    {
                        Id = customer.Id,
                        FirstName = customer.FirstName,
                        LastName = customer.LastName,
                        Email = customer.Email,
                        Discount = customer.Discount,
                        CanBeRemoved = customer.CanBeRemoved
                    };
                    await responseStream.WriteAsync(customerMessage);
                }
            }
            catch (RepositoryException ex)
            {
                //log to ba added
            }
            catch (Exception ex)
            {
                //log to ba added
            }
        }

        /// <summary>
        /// This method adds a customer to the database.
        /// </summary>
        /// <param name="request">Request with customer information</param>
        /// <param name="context">This is the server call context</param>
        /// <returns></returns>
        public override async Task<CustomerResponse> AddCustomer(CustomerModel request, ServerCallContext context)
        {
            try
            {
                var customer = new Customer
                {
                    FirstName = request.FirstName,
                    LastName = request.LastName,
                    Email = request.Email,
                    Discount = request.Discount,
                    CanBeRemoved = request.CanBeRemoved
                };
                await _customerRepository.AddCustomerAsync(customer);
                return new CustomerResponse { Status = Status.Success, Message = "Customer added successfully." };
            }
            catch (RepositoryException ex)
            {
                return new CustomerResponse { Status = Status.Error, Message = ex.Message };
            }
            catch (Exception ex)
            {
                return new CustomerResponse { Status = Status.Error, Message = "An unexpected error occurred: " + ex.Message };
            }
        }

        /// <summary>
        /// This mehtod updates a customer in the database.
        /// </summary>
        /// <param name="request">Request with customer information.</param>
        /// <param name="context">This is the server call context</param>
        /// <returns></returns>
        public override async Task<CustomerResponse> UpdateCustomer(CustomerModel request, ServerCallContext context)
        {
            try
            {
                var customer = new Customer
                {
                    Id = request.Id,
                    FirstName = request.FirstName,
                    LastName = request.LastName,
                    Email = request.Email,
                    Discount = request.Discount,
                    CanBeRemoved = request.CanBeRemoved
                };
                await _customerRepository.UpdateCustomerAsync(customer);
                return new CustomerResponse { Status = Status.Success, Message = "Customer updated successfully." };
            }
            catch (RepositoryException ex)
            {
                return new CustomerResponse { Status = Status.Error, Message = ex.Message };
            }
            catch (Exception ex)
            {
                return new CustomerResponse { Status = Status.Error, Message = "An unexpected error occurred: " + ex.Message };
            }

        }

        /// <summary>
        /// This method deletes a customer from the database.
        /// </summary>
        /// <param name="request">Request with customer information.</param>
        /// <param name="context">This is the server call context.</param>
        /// <returns></returns>
        public override async Task<CustomerResponse> DeleteCustomer(CustomerId request, ServerCallContext context)
        {
            try
            {
                await _customerRepository.DeleteCustomerAsync(request.Id);
                return new CustomerResponse { Status = Status.Success, Message = "Customer deleted successfully." };
            }
            catch (RepositoryException ex)
            {
                return new CustomerResponse { Status = Status.Error, Message = ex.Message };
            }
            catch (Exception ex)
            {
                return new CustomerResponse { Status = Status.Error, Message = "An unexpected error occurred: " + ex.Message };
            }
        }
    }
}
