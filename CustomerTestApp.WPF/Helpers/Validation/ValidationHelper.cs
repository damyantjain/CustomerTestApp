using System.Collections;
using System.ComponentModel;

namespace CustomerTestApp.WPF.Helpers.Validation
{
    /// <summary>
    /// A validation helper class implementing the IValidationHelper and providing useful methods. 
    /// </summary>
    public class ValidationHelper : IValidationHelper
    {

        private readonly Dictionary<string, List<string>> _errorsDictionary = new Dictionary<string, List<string>>();

        public bool HasErrors => _errorsDictionary.Any();

        public event EventHandler<DataErrorsChangedEventArgs>? ErrorsChanged;

        public void AddError(string propertyName, string errorMessage)
        {
            if (!_errorsDictionary.ContainsKey(propertyName))
            {
                _errorsDictionary[propertyName] = new List<string>();
            }
            _errorsDictionary[propertyName].Add(errorMessage);
            OnErrorsChanged(propertyName);
        }

        public void ClearErrors(string propertyName)
        {
            if (_errorsDictionary.ContainsKey(propertyName))
            {
                _errorsDictionary.Remove(propertyName);
                OnErrorsChanged(propertyName);
            }
        }

        private void OnErrorsChanged(string propertyName)
        {
            ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(propertyName));
        }

        public IEnumerable GetErrors(string? propertyName)
        {
            return _errorsDictionary.GetValueOrDefault(propertyName, null) ?? new List<string>();
        }
    }
}
