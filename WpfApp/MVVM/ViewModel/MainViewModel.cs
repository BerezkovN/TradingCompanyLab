using WpfApp.MVVM.Core;

namespace WpfApp.MVVM.ViewModel
{
    public class MainViewModel : ViewModelBase
    {

        private readonly Navigator _navigator;

        private ViewModelBase _currentViewModel;

        public ViewModelBase CurrentViewModel {
            get => _currentViewModel;
            set
            {
                
                _currentViewModel = value;
                OnPropertyChange();
            }
        }

        public MainViewModel(Navigator navigator)
        {
            _navigator = navigator;
        }
    }
}
