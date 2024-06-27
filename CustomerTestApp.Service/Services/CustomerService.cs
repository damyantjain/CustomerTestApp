using CustomerTestApp.Service.Models;
using Grpc.Core;
using Microsoft.EntityFrameworkCore;

namespace CustomerTestApp.Service.Services
{
    public class CustomerService : CustomerManagement.CustomerManagementBase
    {
        private readonly CustomerContext _context;

        /// <summary>
        /// The customer context is injected here.
        /// </summary>
        /// <param name="context"></param>
        public CustomerService(CustomerContext context)
        {
            _context = context;
        }

        /// <summary>
        /// This method returns all customers from the database.
        /// </summary>
        /// <param name="request">This is an empty request</param>
        /// <param name="responseStream">This is the response stream having the customer object</param>
        /// <param name="context">This is the server call context</param>
        /// <returns></returns>
        public override async Task GetAllCustomers(Empty request, IServerStreamWriter<Customer> responseStream, ServerCallContext context)
        {
            await foreach(var customer in _context.Customers.AsAsyncEnumerable())
            {
                var customerMessage = new Customer
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

        /// <summary>
        /// This method adds a customer to the database.
        /// </summary>
        /// <param name="request">Request with customer information</param>
        /// <param name="context">This is the server call context</param>
        /// <returns></returns>
        public override async Task<CustomerResponse> AddCustomer(Customer request, ServerCallContext context)
        {
            var response = new CustomerResponse();
            try
            {
                var customer = new Models.Customer
                {
                    FirstName = request.FirstName,
                    LastName = request.LastName,
                    Email = request.Email,
                    Discount = request.Discount,
                    CanBeRemoved = request.CanBeRemoved
                };
                _context.Customers.Add(customer);
                await _context.SaveChangesAsync();

                response.Status = Status.Success;
                response.Message = "Customer added successfully";
            }
            catch (Exception ex)
            {
                //Log exception
                response.Status = Status.Error;
                response.Message = "Something went wrong";
            }
            return response;
        }

        /// <summary>
        /// This mehtod updates a customer in the database.
        /// </summary>
        /// <param name="request">Request with customer information.</param>
        /// <param name="context">This is the server call context</param>
        /// <returns></returns>
        public override async Task<CustomerResponse> UpdateCustomer(Customer request, ServerCallContext context)
        {
            var response = new CustomerResponse();
            try
            {
                var existingCustomer = await _context.Customers.FindAsync(request.Id);
                if (existingCustomer != null)
                {
                    existingCustomer.FirstName = request.FirstName;
                    existingCustomer.LastName = request.LastName;
                    existingCustomer.Email = request.Email;
                    existingCustomer.Discount = request.Discount;
                    existingCustomer.CanBeRemoved = request.CanBeRemoved;

                    await _context.SaveChangesAsync();

                    response.Status = Status.Success;
                    response.Message = "Customer updated successfully";
                }
                else
                {
                    response.Status = Status.Error;
                    response.Message = "Customer not found";
                }
            }
            catch (Exception ex)
            {
                // Log the exception
                response.Status = Status.Error;
                response.Message = "Internal server error";
            }
            return response;
        }

        /// <summary>
        /// This method deletes a customer from the database.
        /// </summary>
        /// <param name="request">Request with customer information.</param>
        /// <param name="context">This is the server call context.</param>
        /// <returns></returns>
        public override async Task<CustomerResponse> DeleteCustomer(CustomerId request, ServerCallContext context)
        {
            var response = new CustomerResponse();
            try
            {
                var existingCustomer = await _context.Customers.FindAsync(request.Id);
                if (existingCustomer != null && existingCustomer.CanBeRemoved)
                {
                    _context.Customers.Remove(existingCustomer);
                    await _context.SaveChangesAsync();

                    response.Status = Status.Success;
                    response.Message = "Customer deleted successfully";
                }
                else
                {
                    response.Status = Status.Error;
                    response.Message = "Customer not found or cannot be removed";
                }
            }
            catch (Exception ex)
            {
                // Log the exception
                response.Status = Status.Error;
                response.Message = "Internal server error";
            }
            return response;
        }
    }
}
