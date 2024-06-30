using CustomerTestApp.WPF.Models;
using System.Collections.ObjectModel;
using CustomerTestApp.WPF.Messages;
using CommunityToolkit.Mvvm.Input;
using System.Windows;
using CustomerTestApp.WPF.Helpers.Messenger;
using CustomerTestApp.WPF.Services;
using CustomerTestApp.Service;

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

        private CancellationTokenSource _cancellationTokenSource;

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
                _ = LoadCustomersAsync();
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
                _ = LoadCustomersAsync();
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
            SearchText = "";
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
        private async Task LoadCustomersAsync()
        {
            _cancellationTokenSource?.Cancel();
            _cancellationTokenSource = new CancellationTokenSource();
            var token = _cancellationTokenSource.Token;

            FilteredCustomerList.Clear();
            await foreach (var customer in _customerService.GetAllCustomersAsync(SelectedFilter, SearchText, token).WithCancellation(token))
            {
                FilteredCustomerList.Add(new Customer
                {
                    Id = Guid.Parse(customer.Id),
                    FirstName = customer.FirstName,
                    LastName = customer.LastName,
                    Email = customer.Email,
                    Discount = customer.Discount,
                    CanBeRemoved = customer.CanBeRemoved
                });
            }
        }

        private async Task SaveCustomer(Customer customer)
        {
            if (customer.Id == null)
            {
                await AddNewCustomer(customer);
            }
            else
            {
                await UpdateCustomer(customer);
            }
            SelectedCustomer = null;
        }

        private async Task AddNewCustomer(Customer customer)
        {
            var newCustomer = new CustomerModel
            {
                FirstName = customer.FirstName,
                LastName = customer.LastName,
                Email = customer.Email,
                Discount = customer.Discount,
                CanBeRemoved = true
            };

            var response = await _customerService.AddCustomerAsync(newCustomer);

            if (response.Status == Status.Success)
            {
                await LoadCustomersAsync();
            }
            else
            {
                MessageBox.Show(response.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private async Task UpdateCustomer(Customer customer)
        {
            var updatedCustomer = new CustomerModel
            {
                Id = customer.Id.ToString(),
                FirstName = customer.FirstName,
                LastName = customer.LastName,
                Email = customer.Email,
                Discount = customer.Discount,
                CanBeRemoved = customer.CanBeRemoved
            };

            var response = await _customerService.UpdateCustomerAsync(updatedCustomer);

            if (response.Status == Status.Success)
            {
                await LoadCustomersAsync();
            }
            else
            {
                MessageBox.Show(response.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private async void RemoveCustomer(Customer customer)
        {
            if (customer != null && customer.Id.HasValue)
            {
                var result = MessageBox.Show($"Are you sure you want to delete {customer.FirstName} {customer.LastName}?", "Confirm Delete", MessageBoxButton.YesNo, MessageBoxImage.Warning);
                if (result == MessageBoxResult.Yes)
                {
                    SelectedCustomer = null;
                    var response = await _customerService.DeleteCustomerAsync(customer.Id.Value);

                    if (response.Status == Status.Success)
                    {
                        await LoadCustomersAsync();
                    }
                    else
                    {
                        MessageBox.Show(response.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
        }

        #endregion
    }
}
