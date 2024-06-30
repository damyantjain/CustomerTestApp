using CommunityToolkit.Mvvm.Input;
using CustomerTestApp.WPF.Helpers.Messenger;
using CustomerTestApp.WPF.Helpers.Validation;
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

        private string _discount;

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
        public string Id => EditableCustomer?.Id?.ToString() ?? string.Empty;

        /// <summary>
        /// The first name of the Customer.
        /// </summary>
        public string FirstName
        {
            get => _firstName;
            set
            {
                _firstName = value;
                ValidateFirstName();
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
                ValidateLastName();
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
                ValidateEmail();
                OnPropertyChanged(nameof(Email));
            }
        }

        /// <summary>
        /// The discount of the customer.
        /// </summary>
        public string Discount
        {
            get => _discount;
            set
            {
                _discount = value;
                ValidateDiscount();
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
            Messenger.Instance.Register<SelectedCustomerChangedMessage>((m) => EditableCustomer = m.SelectedCustomer?.Clone());
        }

        #region Private Methods

        private async void SaveCustomer()
        {
            if (EditableCustomer != null)
            {
                EditableCustomer.FirstName = FirstName;
                EditableCustomer.LastName = LastName;
                EditableCustomer.Email = Email;
                EditableCustomer.Discount = int.TryParse(Discount, out var discount) ? discount : 0;
                await Messenger.Instance.SendAsync(new SaveCustomerMessage(EditableCustomer));
            }
        }

        private void UpdateCustomerProperties()
        {
            if (EditableCustomer != null)
            {
                OnPropertyChanged(nameof(Id));
                FirstName = EditableCustomer.FirstName;
                LastName = EditableCustomer.LastName;
                Email = EditableCustomer.Email;
                Discount = EditableCustomer.Discount.ToString();
            }
        }

        private void GetValidationHelper_ErrorChanged(object sender, DataErrorsChangedEventArgs e)
        {
            ErrorsChanged?.Invoke(this, e);
            OnPropertyChanged(nameof(CanSave));
        }

        private void ValidateFirstName()
        {
            _validationHelper.ClearErrors(nameof(FirstName));
            if (string.IsNullOrWhiteSpace(FirstName))
            {
                _validationHelper.AddError(nameof(FirstName), "First Name is required.");
            }
        }

        private void ValidateLastName()
        {
            _validationHelper.ClearErrors(nameof(LastName));
            if (string.IsNullOrWhiteSpace(LastName))
            {
                _validationHelper.AddError(nameof(LastName), "Last Name is required.");
            }
        }

        private void ValidateEmail()
        {
            _validationHelper.ClearErrors(nameof(Email));
            if (string.IsNullOrWhiteSpace(Email))
            {
                _validationHelper.AddError(nameof(Email), "Email is required.");
            }
        }

        private void ValidateDiscount()
        {
            _validationHelper.ClearErrors(nameof(Discount));
            if(string.IsNullOrEmpty(Discount) || !int.TryParse(Discount, out var discountValue) || discountValue < 0 || discountValue > 30)
            {
                _validationHelper.AddError(nameof(Discount), "Discount must be between 0 and 30.");
            }
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
