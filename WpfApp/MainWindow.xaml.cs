using System.Windows;
using WpfApp.MVVM.ViewModel;

namespace WpfApp
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            MainViewModel vm = new MainViewModel();
            DataContext = vm;
        }
    }
}