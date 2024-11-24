
using BusinessLogic;
using DTO;
using System.IO;
using System.Windows.Media.Imaging;
using WpfApp.MVVM.Core;

namespace WpfApp.MVVM.ViewModel
{
    public class UserViewModel : ViewModelBase
    {
        
        private readonly MainViewModel _mainViewModel;
        private readonly User _loggedInUser;

        private bool _isEditing = false;


        private BitmapImage _userProfile;
        public BitmapImage UserProfile
        {
            get => _userProfile; 
            set => SetProperty(ref _userProfile, value);
        }

        private bool _isNotEditing;
        public bool IsNotEditing
        {
            get => _isNotEditing;
            set => SetProperty(ref _isNotEditing, value);
        }


        private string _username;
        public string Username
        {
            get => _username;
            set => SetProperty(ref _username, value);
        }

        private string _role;
        public string Role
        {
            get => _role;
            set => SetProperty(ref _role, value);
        }

        private string _email;
        public string Email
        {
            get => _email;
            set => SetProperty(ref _email, value);
        }

        private string _firstName;
        public string FirstName
        {
            get => _firstName;
            set => SetProperty(ref _firstName, value);
        }

        private string _lastName;
        public string LastName
        {
            get => _lastName;
            set => SetProperty(ref _lastName, value);
        }

        private string _gender;
        public string Gender
        {
            get => _gender;
            set => SetProperty(ref _gender, value);
        }

        private string _phoneNumber;
        public string PhoneNumber
        {
            get => _phoneNumber;
            set => SetProperty(ref _phoneNumber, value);
        }

        private string _address;
        public string Address
        {
            get => _address;
            set => SetProperty(ref _address, value);
        }

        private string _editOrUpdateContent;

        public string EditOrUpdateContent
        {
            get => _editOrUpdateContent;
            set => SetProperty(ref _editOrUpdateContent, value);
        }


        public RelayCommand LogoutCommand => new RelayCommand(Logout);
        public RelayCommand EditOrUpdateCommand => new RelayCommand(EditOrUpdate);


        public UserViewModel(MainViewModel mainViewModel)
        {
            _mainViewModel = mainViewModel;
        }

        public void OnNavigate()
        {
            IsNotEditing = true;

            if (_mainViewModel.TradingCompany.LoggedInUser == null)
                return;

            UserData userData = _mainViewModel.TradingCompany.LoggedInUser.Data;
            
            Username = userData.Username;
            Role = userData.Role;
            Email = userData.Email;
            FirstName = userData.FirstName;
            LastName = userData.LastName;
            Gender = userData.Gender;
            PhoneNumber = userData.PhoneNumber;
            Address = userData.Address;

            DisplayImage();
        }

        private void DisplayImage()
        {
            if (_mainViewModel.TradingCompany.LoggedInUser == null)
                return;

            if (_mainViewModel.TradingCompany.LoggedInUser.Data.ProfilePicture == null)
                return;

            var image = LoadImageFromBytes(_mainViewModel.TradingCompany.LoggedInUser.Data.ProfilePicture);
            if (image == null)
                return;

            UserProfile = image;
        }

        private void Logout(object? o)
        {
            // TODO: Logout
            //_mainViewModel.TradingCompany.

            _mainViewModel.Navigate(MainViewModel.Pages.Login);
        }

        private void EditOrUpdate(object? o)
        {
            _isEditing = !_isEditing;

            if (_isEditing)
            {
                IsNotEditing = false;
                EditOrUpdateContent = "Edit";
            }
            else
            {
                IsNotEditing = false;
                EditOrUpdateContent = "Update";

                _mainViewModel.TradingCompany.UpdateUser()
            }


        }

        private static BitmapImage? LoadImageFromBytes(byte[]? imageData)
        {
            if (imageData == null || imageData.Length == 0)
                return null;

            using (var memoryStream = new MemoryStream(imageData))
            {
                var bitmapImage = new BitmapImage();
                bitmapImage.BeginInit();
                bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                bitmapImage.StreamSource = memoryStream;
                bitmapImage.EndInit();
                bitmapImage.Freeze();
                return bitmapImage;
            }
        }
    }
}
