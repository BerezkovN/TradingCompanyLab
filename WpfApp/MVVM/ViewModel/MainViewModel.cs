using WpfApp.MVVM.Core;

namespace WpfApp.MVVM.ViewModel
{
    public class MainViewModel : ViewModelBase
    {

        private readonly LoginViewModel _loginViewMode;
        private readonly UserViewModel _userViewModel;

        private ViewModelBase _currentViewModel;

        public ViewModelBase CurrentViewModel {
            get => _currentViewModel;
            set {
                _currentViewModel = value;
                OnPropertyChange();
            }
        }

        public MainViewModel()
        {
            _loginViewMode = new LoginViewModel(this);
            _userViewModel = new UserViewModel(this);

            this.Navigate(Pages.Login);
        }


        public void Navigate(MainViewModel.Pages page)
        {

            ViewModelBase navigatedViewModel;

            switch (page)
            {
                case Pages.Login:
                    navigatedViewModel = _loginViewMode;
                    break;
                case Pages.UserMenu:
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
            UserMenu,
            AdminMenu
        }
    }
}
