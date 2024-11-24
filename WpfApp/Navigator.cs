using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfApp.MVVM.Core;
using WpfApp.MVVM.ViewModel;

namespace WpfApp
{
    public class Navigator : ViewModelBase
    {
        private readonly MainWindow _mainWindow;

        private readonly MainViewModel _mainViewModel;
        private readonly LoginViewModel _loginViewMode;
        private readonly UserViewModel _userViewModel;

        //private ViewModelBase _currentViewModel;

        //public ViewModelBase CurrentViewModel
        //{
        //    get => _currentViewModel;
        //    set
        //    {
        //        _currentViewModel = value;
        //    }
        //}

        public Navigator(MainWindow mainWindow)
        {
            _mainWindow = mainWindow;

            _mainViewModel = new MainViewModel(this);
            _loginViewMode = new LoginViewModel(this);
            _userViewModel = new UserViewModel(this);

            _mainWindow.DataContext = _mainViewModel;

            this.Navigate(Pages.Login);
        }

        public void Navigate(Navigator.Pages page)
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

            _mainViewModel.CurrentViewModel = navigatedViewModel;
            //CurrentViewModel = navigatedViewModel;
        }

        public enum Pages
        {
            Login,
            UserMenu,
            AdminMenu
        }
    }
}
