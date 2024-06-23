using CommunityToolkit.Mvvm.Messaging;
using CustomerTestApp.WPF.Models;
using System.Collections.ObjectModel;
using CustomerTestApp.WPF.Messages;
using CommunityToolkit.Mvvm.Input;

namespace CustomerTestApp.WPF.ViewModels
{
    /// <summary>
    /// The Customer Data View Model handles the customer list.
    /// </summary>
    public class CustomerDataViewModel : BaseViewModel
    {

        #region Private Members

        private Customer _selectedCustomer;

        #endregion


        #region Public Properties

        /// <summary>
        /// The New Customer Command prepares to create a new customer.
        /// </summary>
        public IRelayCommand NewCustomerCommand { get; }

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
                OnPropertyChanged(nameof(SelectedCustomer));
                WeakReferenceMessenger.Default.Send(new SelectedCustomerChangedMessage(_selectedCustomer));
            }
        }

        #endregion

        /// <summary>
        /// The Customer Data View Model constructor initializes the customer list.
        /// </summary>
        public CustomerDataViewModel() 
        {
            CustomerList = new ObservableCollection<Customer>();
            LoadCustomers();
            NewCustomerCommand = new RelayCommand(CreateNewCustomer);
            RemoveCustomerCommand = new RelayCommand<Customer>(RemoveCustomer);
            WeakReferenceMessenger.Default.Register<SaveCustomerMessage>(this, (r, m) =>
            {
                SaveCustomer(m.Customer);
            });
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

        private void SaveCustomer(Customer customer)
        {
            if (customer.Id == 0)
            {
                //Later we will let Sqlite handle it.
                customer.Id = CustomerList.Count + 1;
                CustomerList.Add(customer);
            }
            else
            {
                //Later will change to a service call to update.
                var existingCustomer = CustomerList.FirstOrDefault(c => c.Id == customer.Id);
                if (existingCustomer != null)
                {
                    existingCustomer.FirstName = customer.FirstName;
                    existingCustomer.LastName = customer.LastName;
                    existingCustomer.Email = customer.Email;
                    existingCustomer.Discount = customer.Discount;
                }
            }
            //Service call to get the latest data.

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
