using CommunityToolkit.Mvvm.Input;
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
        /// The save customer command saves the selected customer.
        /// </summary>
        public IRelayCommand SaveCustomerCommand { get; }

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

        #region Public Methods

        /// <summary>
        /// The CustomerEditViewModel constructor register the Messenger.
        /// </summary>
        public CustomerEditViewModel()
        {
            SaveCustomerCommand = new RelayCommand(SaveCustomer);
            WeakReferenceMessenger.Default.Register<SelectedCustomerChangedMessage>(this, (r, m) =>
            {
                SelectedCustomer = m.SelectedCustomer;
            });
        }

        #endregion

        #region Private Methods

        private void SaveCustomer()
        {
            if (SelectedCustomer != null)
            {
                WeakReferenceMessenger.Default.Send(new SaveCustomerMessage(SelectedCustomer));
            }
        }

        #endregion
    }
}
