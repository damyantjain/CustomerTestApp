using System.Windows.Controls;

namespace CustomerTestApp.WPF.Views
{
    /// <summary>
    /// Interaction logic for CustomerView.xaml
    /// </summary>
    public partial class CustomerListView : UserControl
    {
        public CustomerListView()
        {
            InitializeComponent();
            DataContext = ViewModelLocator.Instance.CustomerDataViewModel;
        }
    }
}
