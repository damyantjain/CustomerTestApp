using CustomerTestApp.WPF.Models;

namespace CustomerTestApp.WPF.Messages
{
    /// <summary>
    /// The SelectedCustomerChangedMessage class creates a message for the selected customer.
    /// </summary>
    public class SelectedCustomerChangedMessage : BaseMessage
    {
        /// <summary>
        /// The Selected Customer
        /// </summary>
        public Customer SelectedCustomer { get; }

        /// <summary>
        /// The SelectedCustomerChangedMessage constructor initializes the selected customer.
        /// </summary>
        /// <param name="selectedCustomer"></param>
        public SelectedCustomerChangedMessage(Customer selectedCustomer)
        {
            SelectedCustomer = selectedCustomer;
        }
    }
}
