using WpfApp.MVVM.Core;

namespace WpfApp.MVVM.ViewModel
{
    public class UserViewModel : ViewModelBase
    {
        
        private readonly Navigator _navigator;

        public UserViewModel(Navigator navigator)
        {
            _navigator = navigator;
        }

    }
}
