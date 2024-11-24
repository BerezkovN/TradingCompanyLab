using System.Windows;
using WpfApp.MVVM.Core;

namespace WpfApp.MVVM.ViewModel
{
    public class LoginViewModel : ViewModelBase
    {
        RelayCommand LoginCommand => new RelayCommand(Login);

        private MainViewModel _mainViewModel;

        public LoginViewModel(MainViewModel mainViewModel)
        {
            _mainViewModel = mainViewModel;
        }


        private void Login(object? o)
        {
            MessageBox.Show("funny");
            _mainViewModel.CurrentViewModel = new UserViewModel(_mainViewModel);
        }
    }
}
