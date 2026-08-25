using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using System.Windows.Navigation;
using System.Windows.Shapes;
using XK3118T1Soft.ViewModel;

namespace XK3118T1Soft.View
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly MainViewModel _viewModel;
        public string Weight;
        public MainWindow ()
        {
            InitializeComponent();
            _viewModel = new MainViewModel();

            DataContext = _viewModel;

            (_viewModel.StartCommand as RelayCommand)?.RaiseCanExecuteChanged();
        }
       
    }
}