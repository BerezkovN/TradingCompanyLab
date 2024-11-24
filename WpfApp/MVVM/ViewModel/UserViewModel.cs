using WpfApp.MVVM.Core;

namespace WpfApp.MVVM.ViewModel
{
    public class UserViewModel : ViewModelBase
    {
        
        private readonly MainViewModel _mainViewModel;

        public UserViewModel(MainViewModel mainViewModel)
        {
            _mainViewModel = mainViewModel;
        }

    }
}
