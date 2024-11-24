using BusinessLogic;
using WpfApp.MVVM.Core;

namespace WpfApp.MVVM.ViewModel
{
    public class MainViewModel : ViewModelBase
    {

        private readonly TradingCompany _tradingCompany;

        private readonly LoginViewModel _loginViewMode;
        private readonly UserViewModel _userViewModel;
        private readonly RecoverPasswordViewModel _recoverPasswordViewModel;

        private ViewModelBase _currentViewModel;

        public MainViewModel()
        {
            _tradingCompany = new TradingCompany();

            _loginViewMode = new LoginViewModel(this);
            _userViewModel = new UserViewModel(this);
            _recoverPasswordViewModel = new RecoverPasswordViewModel(this);

            this.Navigate(Pages.Login);
        }

        public TradingCompany TradingCompany => _tradingCompany;

        public ViewModelBase CurrentViewModel
        {
            get => _currentViewModel;
            set
            {
                _currentViewModel = value;
                OnPropertyChange();
            }
        }


        public void Navigate(MainViewModel.Pages page)
        {

            ViewModelBase navigatedViewModel;

            switch (page)
            {
                case Pages.Login:
                    navigatedViewModel = _loginViewMode;
                    break;
                case Pages.RecoverPassword:
                    navigatedViewModel = _recoverPasswordViewModel;
                    break;
                case Pages.UserMenu:
                    _userViewModel.OnNavigate();
                    navigatedViewModel = _userViewModel;
                    break;
                case Pages.AdminMenu:
                    return;
                    //navigatedViewModel = _userViewModel;
                    break;
                default:
                    return;
                    break;
            }

            this.CurrentViewModel = navigatedViewModel;
        }

        public enum Pages
        {
            Login,
            RecoverPassword,
            UserMenu,
            AdminMenu
        }
    }
}
