using System.Windows;
using System.Windows.Input;
using WpfApp.MVVM.Core;

namespace WpfApp.MVVM.ViewModel
{
    public class LoginViewModel : ViewModelBase
    {

        public RelayCommand LoginCommand => new RelayCommand(Login);

        private string _username;
        public string Username {
            get => _username; 
            set
            {
                _username = value;
                OnPropertyChange();
            } 
        }

        private string _password;
        public string Password
        {
            get => _password;
            set
            {
                _password= value;
                OnPropertyChange();
            }
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
                "Would you like to reset password?", "Invalid username or password", 
                MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (Result == MessageBoxResult.Yes)
            {
                _mainViewModel.Navigate(MainViewModel.Pages.RecoverPassword);
            }
        }
    }
}
