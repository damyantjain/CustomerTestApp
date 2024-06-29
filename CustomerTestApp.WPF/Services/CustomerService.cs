using CustomerTestApp.Service;
using Grpc.Core;
using Grpc.Net.Client;
using Microsoft.Extensions.Configuration;

namespace CustomerTestApp.WPF.Services
{
    /// <summary>
    /// The Customer Service class handles the gRPC calls to the server for customers.
    /// </summary>
    public class CustomerService : ICustomerService
    {
        private readonly CustomerManagement.CustomerManagementClient _client;

        public CustomerService(IConfiguration configuration)
        {
            var address = configuration["GrpcServer:Address"];
            var channel = GrpcChannel.ForAddress(address);
            _client = new CustomerManagement.CustomerManagementClient(channel);
        }

        public async IAsyncEnumerable<CustomerModel> GetAllCustomersAsync(FilterType filterType, string searchText)
        {
            var request = new CustomerFilter
            {
                FilterType = filterType,
                SearchText = searchText
            };

            using var call = _client.GetAllCustomers(request);
            await foreach (var customer in call.ResponseStream.ReadAllAsync())
            {
                yield return customer;
            }
        }

        public async Task<CustomerResponse> AddCustomerAsync(CustomerModel customer)
        {
            try
            {
                return await _client.AddCustomerAsync(customer);
            }
            catch (RpcException ex)
            {
                return new CustomerResponse { Status = Service.Status.Error, Message = $"gRPC error: {ex.Status.Detail}" };
            }
            catch (Exception ex)
            {
                return new CustomerResponse { Status = Service.Status.Error, Message = $"An error occurred: {ex.Message}" };
            }
        }

        public async Task<CustomerResponse> UpdateCustomerAsync(CustomerModel customer)
        {
            try
            {
                return await _client.UpdateCustomerAsync(customer);
            }
            catch (RpcException ex)
            {
                return new CustomerResponse { Status = Service.Status.Error, Message = $"gRPC error: {ex.Status.Detail}" };
            }
            catch (Exception ex)
            {
                return new CustomerResponse { Status = Service.Status.Error, Message = $"An error occurred: {ex.Message}" };
            }
        }

        public async Task<CustomerResponse> DeleteCustomerAsync(int customerId)
        {
            try
            {
                var request = new CustomerId { Id = customerId };
                return await _client.DeleteCustomerAsync(request);
            }
            catch (RpcException ex)
            {
                return new CustomerResponse { Status = Service.Status.Error, Message = $"gRPC error: {ex.Status.Detail}" };
            }
            catch (Exception ex)
            {
                return new CustomerResponse { Status = Service.Status.Error, Message = $"An error occurred: {ex.Message}" };
            }
        }

    }
}