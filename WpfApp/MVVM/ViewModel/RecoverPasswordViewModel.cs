using Accessibility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using WpfApp.MVVM.Core;

namespace WpfApp.MVVM.ViewModel
{
    public class RecoverPasswordViewModel : ViewModelBase
    {

        private readonly MainViewModel _mainViewModel;


        private Visibility _recoverPassword;
        public Visibility RecoverPassword
        {
            get => _recoverPassword;
            set => SetProperty(ref _recoverPassword, value);
        }

        private string _recoveryKey;
        public string RecoveryKey
        {
            get => _recoveryKey;
            set => SetProperty(ref _recoveryKey, value);
        }

        private string _newPassword;
        public string NewPassword
        {
            get => _newPassword;
            set => SetProperty(ref _newPassword, value);
        }

        public RelayCommand RecoverCommand => new RelayCommand(Recover);
        public RelayCommand ResetPasswordCommand => new RelayCommand(ResetPassword);
        public RelayCommand GoBackCommand => new RelayCommand(GoBack);

        public RecoverPasswordViewModel(MainViewModel main)
        {
            _mainViewModel = main;
            RecoverPassword = Visibility.Collapsed;
        }

        private void Recover(object? o)
        {
            bool result = _mainViewModel.TradingCompany.CheckRecoveryKey(RecoveryKey);

            if (result)
            {
                RecoverPassword = Visibility.Visible;
            }
            else
            {
                MessageBox.Show("Invalid recovery key");
            }
        }

        private void ResetPassword(object? o)
        {
            bool result = _mainViewModel.TradingCompany.CheckRecoveryKey(RecoveryKey);

            if (!result)
            {
                MessageBox.Show("Invalid recovery key");
                return;
            }

            _mainViewModel.TradingCompany.UpdatePassword(RecoveryKey, NewPassword);
            MessageBox.Show("Password updated successfully");
            _mainViewModel.Navigate(MainViewModel.Pages.Login);
        }

        private void GoBack(object? o)
        {
            _mainViewModel.Navigate(MainViewModel.Pages.Login);
        }

    }
}
