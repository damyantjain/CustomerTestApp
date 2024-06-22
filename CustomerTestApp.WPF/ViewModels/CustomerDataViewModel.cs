using CommunityToolkit.Mvvm.Messaging;
using CustomerTestApp.WPF.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using CustomerTestApp.WPF.Messages;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        }


        #region Private Methods

        /// <summary>
        /// The LoadCustomers method loads the customers into the customer list.
        /// </summary>
        private void LoadCustomers()
        {
            CustomerList.Add(new Customer { Id = 1, FirstName = "Damyant", LastName = "Jain", Email = "dj@example.com", Discount = 10 });
            CustomerList.Add(new Customer { Id = 2, FirstName = "Sukriti", LastName = "Gantayet", Email = "sg@example.com", Discount = 15 });
        }

        #endregion
    }
}
