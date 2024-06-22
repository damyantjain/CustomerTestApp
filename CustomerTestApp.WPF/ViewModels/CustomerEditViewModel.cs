using CommunityToolkit.Mvvm.Messaging;
using CustomerTestApp.WPF.Messages;
using CustomerTestApp.WPF.Models;

namespace CustomerTestApp.WPF.ViewModels
{
    /// <summary>
    /// The CustomerEditViewModel handles the selected customer's data.
    /// </summary>
    public class CustomerEditViewModel : BaseViewModel
    {
        #region Private Members

        private Customer _selectedCustomer;

        #endregion

        #region Public Properties

        /// <summary>
        /// The Selected Customer
        /// </summary>
        public Customer SelectedCustomer
        {
            get => _selectedCustomer;
            set
            {
                _selectedCustomer = value;
                OnPropertyChanged(nameof(SelectedCustomer));
            }
        }

        #endregion

        /// <summary>
        /// The CustomerEditViewModel constructor register the Messenger.
        /// </summary>
        public CustomerEditViewModel()
        {
            WeakReferenceMessenger.Default.Register<SelectedCustomerChangedMessage>(this, (r, m) =>
            {
                SelectedCustomer = m.SelectedCustomer;
            });
        }
    }
}
