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

        private Customer _editableCustomer;

        #endregion

        #region Public Properties

        /// <summary>
        /// The save customer command saves the selected customer.
        /// </summary>
        public IRelayCommand SaveCustomerCommand { get; }

        /// <summary>
        /// The Selected Customer
        /// </summary>
        public Customer EditableCustomer
        {
            get => _editableCustomer;
            set
            {
                _editableCustomer = value;
                OnPropertyChanged(nameof(EditableCustomer));
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
                EditableCustomer = m.SelectedCustomer.Clone();
            });
        }

        #endregion

        #region Private Methods

        private void SaveCustomer()
        {
            if (EditableCustomer != null)
            {
                WeakReferenceMessenger.Default.Send(new SaveCustomerMessage(EditableCustomer));
            }
        }

        #endregion
    }
}
