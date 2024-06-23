using CustomerTestApp.WPF.Models;

namespace CustomerTestApp.WPF.Messages
{
    public class SaveCustomerMessage
    {
        public Customer Customer { get; }

        public SaveCustomerMessage(Customer customer)
        {
            Customer = customer;
        }
    }
}