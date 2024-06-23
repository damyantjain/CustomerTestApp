using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using CustomerTestApp.WPF.Helpers;
using CustomerTestApp.WPF.Messages;
using CustomerTestApp.WPF.Models;
using System.Collections;
using System.ComponentModel;

namespace CustomerTestApp.WPF.ViewModels
{
    /// <summary>
    /// The CustomerEditViewModel handles the selected customer's data.
    /// </summary>
    public class CustomerEditViewModel : BaseViewModel
    {
        #region Private Members

        private Customer _editableCustomer;

        private readonly ValidationHelper _validationHelper;

        private string _firstName;

        private string _lastName;

        private string _email;

        private int _discount;

        #endregion

        #region Public Properties

        /// <summary>
        /// The save customer command.
        /// </summary>
        public IRelayCommand SaveCustomerCommand { get; }

        /// <summary>
        /// The customer object for editing.
        /// </summary>
        public Customer EditableCustomer
        {
            get => _editableCustomer;
            set
            {
                _editableCustomer = value;
                OnPropertyChanged(nameof(EditableCustomer));
                OnPropertyChanged(nameof(IsCustomerSelected));
                UpdateCustomerProperties();
            }
        }

        /// <summary>
        /// The Id of the customer.
        /// </summary>
        public int Id => EditableCustomer?.Id ?? 0;

        /// <summary>
        /// The first name of the Customer.
        /// </summary>
        public string FirstName
        {
            get => _firstName;
            set
            {
                _firstName = value;
                _validationHelper.ClearErrors(nameof(FirstName));
                if (string.IsNullOrWhiteSpace(_firstName))
                {
                    _validationHelper.AddError(nameof(FirstName), "First Name is required");
                }
                OnPropertyChanged(nameof(FirstName));
            }
        }

        /// <summary>
        /// The last name of the customer
        /// </summary>
        public string LastName
        {
            get => _lastName;
            set
            {
                _lastName = value;
                _validationHelper.ClearErrors(nameof(LastName));
                if (string.IsNullOrWhiteSpace(_lastName))
                {
                    _validationHelper.AddError(nameof(LastName), "Last Name is required");
                }
                OnPropertyChanged(nameof(LastName));
            }
        }

        /// <summary>
        /// The Email of the customer.
        /// </summary>
        public string Email
        {
            get => _email;
            set
            {
                _email = value;
                _validationHelper.ClearErrors(nameof(Email));
                if (string.IsNullOrWhiteSpace(_email))
                {
                    _validationHelper.AddError(nameof(Email), "Email is required");
                }
                OnPropertyChanged(nameof(Email));
            }
        }

        /// <summary>
        /// The discount of the customer.
        /// </summary>
        public int Discount
        {
            get => _discount;
            set
            {
                _discount = value;
                _validationHelper.ClearErrors(nameof(Discount));
                if (_discount < 0m || _discount > 30m)
                {
                    _validationHelper.AddError(nameof(Discount), "Discount must be between 0 and 30");
                }
                OnPropertyChanged(nameof(Discount));
            }
        }

        /// <summary>
        /// Boolean to determine if the user data can be saved
        /// </summary>
        public bool CanSave => !HasErrors;

        /// <summary>
        /// Boolean to determine if there are any errors with validation.
        /// </summary>
        public bool HasErrors => _validationHelper.HasErrors;

        /// <summary>
        /// Event when Error are changed.
        /// </summary>
        public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;

        public bool IsCustomerSelected => EditableCustomer != null;

        #endregion

        public CustomerEditViewModel()
        {
            SaveCustomerCommand = new RelayCommand(SaveCustomer);
            _validationHelper = new ValidationHelper();
            _validationHelper.ErrorsChanged += GetValidationHelper_ErrorChanged;
            WeakReferenceMessenger.Default.Register<SelectedCustomerChangedMessage>(this, (r, m) =>
            {
                EditableCustomer = m.SelectedCustomer?.Clone();
            });
        }

        #region Private Methods

        private void SaveCustomer()
        {
            if (EditableCustomer != null)
            {
                EditableCustomer.FirstName = FirstName;
                EditableCustomer.LastName = LastName;
                EditableCustomer.Email = Email;
                EditableCustomer.Discount = Discount;

                WeakReferenceMessenger.Default.Send(new SaveCustomerMessage(EditableCustomer));
            }
        }

        private void UpdateCustomerProperties()
        {
            if (EditableCustomer != null)
            {
                FirstName = EditableCustomer.FirstName;
                LastName = EditableCustomer.LastName;
                Email = EditableCustomer.Email;
                Discount = EditableCustomer.Discount;
            }
        }

        private void GetValidationHelper_ErrorChanged(object sender, DataErrorsChangedEventArgs e)
        {
            ErrorsChanged?.Invoke(this, e);
            OnPropertyChanged(nameof(CanSave));
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Returns a list of errors based on a property.
        /// </summary>
        /// <param name="propertyName">Property for which the errors are to be fetched.</param>
        /// <returns>list of errors based on the propertyName</returns>

        public IEnumerable GetErrors(string? propertyName)
        {
            return _validationHelper.GetErrors(propertyName);
        }

        #endregion
    }
}
