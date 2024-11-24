using System.Windows;
using WpfApp.MVVM.Core;

namespace WpfApp.MVVM.ViewModel
{
    public class LoginViewModel : ViewModelBase
    {
        public RelayCommand LoginCommand => new RelayCommand(Login);

        private readonly MainViewModel _mainViewModel;

        public LoginViewModel(MainViewModel mainViewModel)
        {
            _mainViewModel = mainViewModel;
        }


        private void Login(object? o)
        {
            _mainViewModel.Navigate(MainViewModel.Pages.UserMenu);
        }
    }
}
