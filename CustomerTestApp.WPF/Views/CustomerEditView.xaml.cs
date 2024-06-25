using System.Windows.Controls;

namespace CustomerTestApp.WPF.Views
{
    /// <summary>
    /// Interaction logic for CustomerEditView.xaml
    /// </summary>
    public partial class CustomerEditView : UserControl
    {
        public CustomerEditView()
        {
            InitializeComponent();
            DataContext = ViewModelLocator.Instance.CustomerEditViewModel;
        }
    }
}
