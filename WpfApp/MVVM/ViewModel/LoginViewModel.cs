using System.Windows;
using WpfApp.MVVM.Core;

namespace WpfApp.MVVM.ViewModel
{
    public class LoginViewModel : ViewModelBase
    {
        public RelayCommand LoginCommand => new RelayCommand(Login);

        private readonly Navigator _navigator;

        public LoginViewModel(Navigator navigator)
        {
            _navigator = navigator;
        }


        private void Login(object? o)
        {
            _navigator.Navigate(Navigator.Pages.UserMenu);
        }
    }
}
