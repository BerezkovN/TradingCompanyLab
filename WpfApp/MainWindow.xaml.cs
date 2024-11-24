using System.Windows;
using WpfApp.MVVM.ViewModel;

namespace WpfApp
{
    public partial class MainWindow : Window
    {
        private readonly Navigator _navigator;
        
        public MainWindow()
        {
            InitializeComponent();

            _navigator = new Navigator(this);
        }
    }
}