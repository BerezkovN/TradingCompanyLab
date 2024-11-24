using System.Windows;
using WpfApp.MVVM.Core;

namespace WpfApp.MVVM.ViewModel
{
    public class LoginViewModel : ViewModelBase
    {

        public RelayCommand LoginCommand => new RelayCommand(Login);

        private string _username;
        public string Username {
            get => _username; 
            set => SetProperty(ref _username, value);
        }

        private string _password;
        public string Password
        {
            get => _password;
            set => SetProperty(ref _password, value);
        }

        private readonly MainViewModel _mainViewModel;

        public LoginViewModel(MainViewModel mainViewModel)
        {
            _mainViewModel = mainViewModel;
        }


        private void Login(object? o)
        {
            if (Username == null || Password == null)
                return;

            var user = _mainViewModel.TradingCompany.LogIn(Username, Password);

            if (user != null)
            {
                _mainViewModel.Navigate(MainViewModel.Pages.UserMenu);
                return;
            }

            MessageBoxResult Result = MessageBox.Show(
                "Invalid username or password", "Would you like to reset password?", 
                MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (Result == MessageBoxResult.Yes)
            {
                _mainViewModel.Navigate(MainViewModel.Pages.RecoverPassword);
            }
        }
    }
}
