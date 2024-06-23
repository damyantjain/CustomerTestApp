using System.ComponentModel;

namespace CustomerTestApp.WPF.Helpers
{
    /// <summary>
    /// The Interface extending the INotifyDataErrorInfo and adding useful functionalities.
    /// </summary>
    public interface IValidationHelper : INotifyDataErrorInfo
    {
        /// <summary>
        /// Clears all the error.
        /// </summary>
        /// <param name="PropertyName"></param>
        void ClearErrors(string PropertyName);

        /// <summary>
        /// Add a new error.
        /// </summary>
        /// <param name="propertyName"></param>
        /// <param name="errorMessage"></param>
        void AddError(string propertyName, string errorMessage);
    }
}
