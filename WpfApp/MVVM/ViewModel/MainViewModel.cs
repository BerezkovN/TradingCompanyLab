using WpfApp.MVVM.Core;

namespace WpfApp.MVVM.ViewModel
{
    public class MainViewModel : ViewModelBase
    {
        private ViewModelBase _currentViewModel;

        public ViewModelBase CurrentViewModel {
            get => _currentViewModel;
            set
            {
                
                _currentViewModel = value;
                OnPropertyChange();
            }
        }

        public MainViewModel(MainWindow mainWindow)
        {
            CurrentViewModel = new LoginViewModel(this);
        }
    }
}
