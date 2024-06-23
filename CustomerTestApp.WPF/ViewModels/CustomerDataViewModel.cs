using CommunityToolkit.Mvvm.Messaging;
using CustomerTestApp.WPF.Models;
using System.Collections.ObjectModel;
using CustomerTestApp.WPF.Messages;
using CommunityToolkit.Mvvm.Input;
using System.Xml.Linq;

namespace CustomerTestApp.WPF.ViewModels
{
    /// <summary>
    /// The Customer Data View Model handles the customer list.
    /// </summary>
    public class CustomerDataViewModel : BaseViewModel
    {

        #region Private Members

        private Customer _selectedCustomer;

        private string firstName;

        #endregion


        #region Public Properties

        public string FirstName
        {
            get => firstName;
            set
            {
                firstName = value;
                OnPropertyChanged(nameof(FirstName));
            }
        }

        /// <summary>
        /// The New Customer Command prepares to create a new customer.
        /// </summary>
        public IRelayCommand NewCustomerCommand { get; }

        public IRelayCommand SaveCustomerCommand { get; }

        /// <summary>
        /// The Remove Customer Command removes a customer from the list.
        /// </summary>
        public IRelayCommand<Customer> RemoveCustomerCommand { get; }

        /// <summary>
        /// The list of customers.
        /// </summary>
        public ObservableCollection<Customer> CustomerList { get; set; }

        /// <summary>
        /// The selected customer.
        /// </summary>
        public Customer SelectedCustomer
        {
            get => _selectedCustomer;
            set
            {
                _selectedCustomer = value;
                if (_selectedCustomer != null)
                {
                    FirstName = _selectedCustomer.FirstName;
                }
                OnPropertyChanged(nameof(SelectedCustomer));
            }
        }

        #endregion

        private void UpdateFirstName()
        {
            FirstName = SelectedCustomer?.FirstName;
        }

        /// <summary>
        /// The Customer Data View Model constructor initializes the customer list.
        /// </summary>
        public CustomerDataViewModel() 
        {
            CustomerList = new ObservableCollection<Customer>();
            LoadCustomers();
            SaveCustomerCommand = new RelayCommand(SaveCustomer);
            NewCustomerCommand = new RelayCommand(CreateNewCustomer);
            RemoveCustomerCommand = new RelayCommand<Customer>(RemoveCustomer);
        }


        #region Private Methods

        private void CreateNewCustomer()
        {
            SelectedCustomer = null;
            SelectedCustomer = new Customer();
        }

        /// <summary>
        /// The LoadCustomers method loads the customers into the customer list.
        /// </summary>
        private void LoadCustomers()
        {
            CustomerList.Add(new Customer { Id = 1, FirstName = "Damyant", LastName = "Jain", Email = "dj@example.com", Discount = 10 });
            CustomerList.Add(new Customer { Id = 2, FirstName = "Sukriti", LastName = "Gantayet", Email = "sg@example.com", Discount = 15 });
        }

        private void SaveCustomer()
        {
            if (SelectedCustomer != null)
            {
                // Update the specific properties of the selected customer
                SelectedCustomer.FirstName = FirstName;

                // Notify the UI about the update
                OnPropertyChanged(nameof(SelectedCustomer));

                // Optional: If you need to update the entire list (not usually needed for a single property update)
                 OnPropertyChanged(nameof(CustomerList));
            }
        }

        private void RemoveCustomer(Customer customer)
        {
            if (customer != null)
            {
                //Need to add a service call to delete the customer.
                CustomerList.Remove(customer);
            }
        }

        #endregion
    }
}
