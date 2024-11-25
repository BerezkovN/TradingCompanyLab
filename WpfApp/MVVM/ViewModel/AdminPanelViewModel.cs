using DTO;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfApp.MVVM.Core;

namespace WpfApp.MVVM.ViewModel
{
    public class AdminPanelViewModel : ViewModelBase
    {

        private readonly MainViewModel _mainViewModel;

        public ObservableCollection<UserData> UserSource { get; set; }
        public ObservableCollection<BankDetailData> BankDetailSource { get; set; }
        public ObservableCollection<SessionData> SessionsSource { get; set; }

        public RelayCommand GoBackCommand => new RelayCommand(GoBack);

        public AdminPanelViewModel(MainViewModel mainViewModel) 
        { 
            _mainViewModel = mainViewModel;

            UserSource = new ObservableCollection<UserData>();
            var users = _mainViewModel.TradingCompany.GetAllUsers();
            foreach (var user in users)
            { 
                UserSource.Add(user.Data);
            }

            BankDetailSource = new ObservableCollection<BankDetailData>(
                _mainViewModel.TradingCompany.GetAllBankDetails()    
            );

            SessionsSource = new ObservableCollection<SessionData>(
                _mainViewModel.TradingCompany.GetAllUserSessions()
            );
        }

        private void GoBack(object? o)
        {
            _mainViewModel.Navigate(MainViewModel.Pages.UserMenu);
        }
    }
}
