using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfApp.MVVM.ViewModel;

namespace WpfApp
{
    public class Navigator
    {
        private readonly MainWindow _mainWindow;

        private readonly MainViewModel _mainViewModel;
        private readonly LoginViewModel _loginViewMode;
        private readonly UserViewModel _userViewModel;

        public Navigator(MainWindow mainWindow)
        {
            _mainWindow = mainWindow;

            _mainViewModel = new MainViewModel(this);
            _loginViewMode = new LoginViewModel(this);
            _userViewModel = new UserViewModel(this);
        }

        public void Navigate(Navigator.Pages page)
        {


        }

        public enum Pages
        {
            Login,
            UserMenu,
            AdminMenu
        }
    }
}
