using CustomerTestApp.WPF.ViewModels;
using Microsoft.Extensions.DependencyInjection;

namespace CustomerTestApp.WPF
{
    /// <summary>
    /// The ViewModelLocator class manages the instantiation of the view models.
    /// </summary>
    public class ViewModelLocator
    {
        /// <summary>
        /// The singleton instance of the ViewModelLocator class.
        /// </summary>
        public static ViewModelLocator Instance { get; } = new ViewModelLocator();

        private ViewModelLocator() { }

        /// <summary>
        /// The CustomerDataViewModel property gets the CustomerDataViewModel instance.
        /// </summary>
        public CustomerDataViewModel CustomerDataViewModel => App.ServiceProvider.GetRequiredService<CustomerDataViewModel>();

        /// <summary>
        /// The CustomerEditViewModel property gets the CustomerEditViewModel instance.
        /// </summary>
        public CustomerEditViewModel CustomerEditViewModel => App.ServiceProvider.GetRequiredService<CustomerEditViewModel>();
    }
}
