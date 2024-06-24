using CommunityToolkit.Mvvm.Messaging;
using CustomerTestApp.WPF.Models;
using System.Collections.ObjectModel;
using CustomerTestApp.WPF.Messages;
using CommunityToolkit.Mvvm.Input;
using System.Windows;

namespace CustomerTestApp.WPF.ViewModels
{
    /// <summary>
    /// The Customer Data View Model handles the customer list.
    /// </summary>
    public class CustomerDataViewModel : BaseViewModel
    {

        #region Private Members

        private Customer _selectedCustomer;

        private string _searchText;

        private FilterType _selectedFilter;

        private ObservableCollection<Customer> _filteredCustomerList;

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

        /// <summary>
        /// The filtered customer list.
        /// </summary>
        public ObservableCollection<Customer> FilteredCustomerList
        {
            get => _filteredCustomerList;
            set
            {
                _filteredCustomerList = value; 
                OnPropertyChanged(nameof(FilteredCustomerList));
            }
        }

        /// <summary>
        /// The search text for filtering customers.
        /// </summary>
        public string SearchText
        {
            get => _searchText;
            set
            {
                _searchText = value;
                OnPropertyChanged(nameof(SearchText));
                ApplyFilter();
            }
        }

        /// <summary>
        /// The selected filter for filtering customers.
        /// </summary>
        public FilterType SelectedFilter
        {
            get => _selectedFilter;
            set
            {
                _selectedFilter = value;
                OnPropertyChanged(nameof(SelectedFilter));
                ApplyFilter();
            }
        }

        #endregion

        /// <summary>
        /// The Customer Data View Model constructor initializes the customer list.
        /// </summary>
        public CustomerDataViewModel() 
        {
            CustomerList = new ObservableCollection<Customer>();
            FilteredCustomerList = new ObservableCollection<Customer>();
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
            //Service call to get all customers.
            CustomerList.Add(new Customer { Id = 1, FirstName = "Damyant", LastName = "Jain", Email = "dj@example.com", CanBeRemoved = false, Discount = 10 });
            CustomerList.Add(new Customer { Id = 2, FirstName = "Sukriti", LastName = "Gantayet", Email = "sg@example.com", CanBeRemoved = false, Discount = 15 });
            ApplyFilter();
        }

        private void SaveCustomer(Customer customer)
        {
            if(customer.Id == 0)
            {
                AddNewCustomer(customer);
            } 
            else
            {
               UpdateCustomer(customer);
            }
            //LoadCustomers();
            SelectedCustomer = null;
        }

        private void AddNewCustomer(Customer customer)
        {
            //Later we will let Sqlite handle it.
            customer.Id = CustomerList.Any() ? CustomerList.Max(c => c.Id) + 1 : 1;
            customer.CanBeRemoved = true;
            CustomerList.Add(customer);
            ApplyFilter();
            //Service call to add customer.
        }

        private void UpdateCustomer(Customer customer)
        {
            var existingCustomer = CustomerList.FirstOrDefault(c => c.Id == customer.Id);
            if (existingCustomer != null)
            {
                existingCustomer.FirstName = customer.FirstName;
                existingCustomer.LastName = customer.LastName;
                existingCustomer.Email = customer.Email;
                existingCustomer.Discount = customer.Discount;
            }
            //Service call to update customer.

            //For now, we will just update the customer in the list.
            RefreshCustomerList(); 
            ApplyFilter();
        }

        private void RefreshCustomerList()
        {
            var tempList = new ObservableCollection<Customer>(CustomerList);
            CustomerList.Clear();
            foreach (var cust in tempList)
            {
                CustomerList.Add(cust);
            }
            OnPropertyChanged(nameof(CustomerList));
        }

        private void RemoveCustomer(Customer customer)
        {
            if (customer != null)
            {
                var result = MessageBox.Show($"Are you sure you want to delete {customer.FirstName} {customer.LastName}?", "Confirm Delete", MessageBoxButton.YesNo, MessageBoxImage.Warning);
                if (result == MessageBoxResult.Yes)
                {
                    SelectedCustomer = null;
                    //Need to add a service call to delete the customer.
                    CustomerList.Remove(customer);
                    ApplyFilter();
                }
            }
        }

        private void ApplyFilter()
        {
            if (string.IsNullOrEmpty(SearchText))
            { 
                FilteredCustomerList = new ObservableCollection<Customer>(CustomerList);
                return;
            }
            var filteredCustomers = CustomerList.AsEnumerable();
            switch (SelectedFilter)
            {
                case FilterType.Name:
                    filteredCustomers = FilterByName();
                    break;
                case FilterType.Email:
                    filteredCustomers = FilterByEmail();
                    break;
                case FilterType.All:
                    filteredCustomers = FilterAll();
                    break;
            }
            FilteredCustomerList = new ObservableCollection<Customer>(filteredCustomers);
        }

        private IEnumerable<Customer> FilterByName()
        {
            return CustomerList.Where(c => c.FirstName.Contains(SearchText, StringComparison.OrdinalIgnoreCase) 
            || c.LastName.Contains(SearchText, StringComparison.OrdinalIgnoreCase));
        }

        private IEnumerable<Customer> FilterByEmail()
        {
            return CustomerList.Where(c => c.Email.Contains(SearchText, StringComparison.OrdinalIgnoreCase));
        }

        private IEnumerable<Customer> FilterAll()
        {
            return CustomerList.Where(c => c.FirstName.Contains(SearchText, StringComparison.OrdinalIgnoreCase) 
            || c.LastName.Contains(SearchText, StringComparison.OrdinalIgnoreCase) 
            || c.Email.Contains(SearchText, StringComparison.OrdinalIgnoreCase));
        }

        #endregion
    }
}
