using BusinessLogic;
using WpfApp.MVVM.Core;

namespace WpfApp.MVVM.ViewModel
{
    public class MainViewModel : ViewModelBase
    {

        private ViewModelBase _currentViewModel;
        public ViewModelBase CurrentViewModel
        {
            get => _currentViewModel;
            set
            {
                _currentViewModel = value;
                OnPropertyChange();
            }
        }

        private readonly TradingCompany _tradingCompany;
        public TradingCompany TradingCompany => _tradingCompany;



        public MainViewModel()
        {
            _tradingCompany = new TradingCompany("SqlServer");

            
            this.Navigate(Pages.Login);
        }


        public void Navigate(MainViewModel.Pages page)
        {

            ViewModelBase navigatedViewModel;

            switch (page)
            {
                case Pages.Login:
                    navigatedViewModel = new LoginViewModel(this);
                    break;
                case Pages.RecoverPassword:
                    navigatedViewModel = new RecoverPasswordViewModel(this);
                    break;
                case Pages.UserMenu:
                    navigatedViewModel = new UserViewModel(this);
                    break;
                case Pages.AdminPanel:
                    navigatedViewModel = new AdminPanelViewModel(this);
                    break;
                default:
                    return;
            }

            this.CurrentViewModel = navigatedViewModel;
        }

        public enum Pages
        {
            Login,
            RecoverPassword,
            UserMenu,
            AdminPanel
        }
    }
}
