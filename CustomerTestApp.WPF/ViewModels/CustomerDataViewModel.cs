using CustomerTestApp.WPF.Models;
using System.Collections.ObjectModel;
using CustomerTestApp.WPF.Messages;
using CommunityToolkit.Mvvm.Input;
using System.Windows;
using CustomerTestApp.WPF.Helpers.Messenger;
using CustomerTestApp.WPF.Services;

namespace CustomerTestApp.WPF.ViewModels
{
    /// <summary>
    /// The Customer Data View Model handles the customer list.
    /// </summary>
    public class CustomerDataViewModel : BaseViewModel
    {

        #region Private Members

        private readonly ICustomerService _customerService;

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
                Messenger.Instance.Send(new SelectedCustomerChangedMessage(_selectedCustomer));
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
        public CustomerDataViewModel(ICustomerService customerService) 
        {
            _customerService = customerService;
            CustomerList = new ObservableCollection<Customer>();
            FilteredCustomerList = new ObservableCollection<Customer>();
            _ = LoadCustomers();
            NewCustomerCommand = new RelayCommand(CreateNewCustomer);
            RemoveCustomerCommand = new RelayCommand<Customer>(RemoveCustomer);
            Messenger.Instance.Register<SaveCustomerMessage>(async m => await SaveCustomer(m.Customer));
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
        private async Task LoadCustomers()
        {

            CustomerList.Clear();
            await foreach (var customer in _customerService.GetAllCustomers())
            {
                CustomerList.Add(new Customer
                {
                    Id = customer.Id,
                    FirstName = customer.FirstName,
                    LastName = customer.LastName,
                    Email = customer.Email,
                    Discount = customer.Discount,
                    CanBeRemoved = customer.CanBeRemoved
                });
            }
            ApplyFilter();
        }

        private async Task SaveCustomer(Customer customer)
        {
            if (customer.Id == 0)
            {
                await AddNewCustomer(customer);
            }
            else
            {
                await UpdateCustomer(customer);
            }
            SelectedCustomer = null;
            ApplyFilter();
        }

        private async Task AddNewCustomer(Customer customer)
        {
            var newCustomer = new Service.Customer
            {
                FirstName = customer.FirstName,
                LastName = customer.LastName,
                Email = customer.Email,
                Discount = customer.Discount,
                CanBeRemoved = true
            };

            var response = await _customerService.AddCustomerAsync(newCustomer);

            if (response.Status == Service.Status.Success)
            {
                await LoadCustomers();
            }
            else
            {
                MessageBox.Show(response.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private async Task UpdateCustomer(Customer customer)
        {
            var updatedCustomer = new Service.Customer
            {
                Id = customer.Id,
                FirstName = customer.FirstName,
                LastName = customer.LastName,
                Email = customer.Email,
                Discount = customer.Discount,
                CanBeRemoved = customer.CanBeRemoved
            };

            var response = await _customerService.UpdateCustomerAsync(updatedCustomer);

            if (response.Status == Service.Status.Success)
            {
                await LoadCustomers();
            }
            else
            {
                MessageBox.Show(response.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
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

        private async void RemoveCustomer(Customer customer)
        {
            if (customer != null)
            {
                var result = MessageBox.Show($"Are you sure you want to delete {customer.FirstName} {customer.LastName}?", "Confirm Delete", MessageBoxButton.YesNo, MessageBoxImage.Warning);
                if (result == MessageBoxResult.Yes)
                {
                    SelectedCustomer = null;
                    var response = await _customerService.DeleteCustomerAsync(customer.Id);

                    if (response.Status == Service.Status.Success)
                    {
                        await LoadCustomers();
                    }
                    else
                    {
                        MessageBox.Show(response.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
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
            return CustomerList.Where(x =>
            {
                var fullName = $"{x.FirstName} {x.LastName}";
                return fullName.Contains(SearchText, StringComparison.OrdinalIgnoreCase);
            });
        }

        private IEnumerable<Customer> FilterByEmail()
        {
            return CustomerList.Where(x => x.Email.Contains(SearchText, StringComparison.OrdinalIgnoreCase));
        }

        private IEnumerable<Customer> FilterAll()
        {
            return CustomerList.Where(x =>
            {
                var fullName = $"{x.FirstName} {x.LastName}";
                return fullName.Contains(SearchText, StringComparison.OrdinalIgnoreCase)
                || x.Email.Contains(SearchText, StringComparison.OrdinalIgnoreCase);
            });
        }

        #endregion
    }
}
