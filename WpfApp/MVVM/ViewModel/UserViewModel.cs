
using BusinessLogic;
using DAL.AdoNet;
using DTO;
using System.IO;
using System.Windows;
using System.Windows.Media.Imaging;
using WpfApp.MVVM.Core;

namespace WpfApp.MVVM.ViewModel
{
    public class UserViewModel : ViewModelBase
    {
        
        private readonly MainViewModel _mainViewModel;
        private readonly User _loggedInUser;
        private readonly BankDetailData _bankDetailData;

        private BitmapImage _userProfile;
        public BitmapImage UserProfile
        {
            get => _userProfile;
            set
            {
                _userProfile= value;
                OnPropertyChange();
            }
        }

        private bool _isNotEditing;
        public bool IsNotEditing
        {
            get => _isNotEditing;
            set
            {
                _isNotEditing = value;
                OnPropertyChange();
            }
        }

        private string _editOrUpdateContent;
        public string EditOrUpdateContent
        {
            get => _editOrUpdateContent;
            set
            {
                _editOrUpdateContent = value;
                OnPropertyChange();
            }
        }

        private Visibility _addCardButtonVisibility;
        public Visibility AddCardButtonVisibility
        {
            get => _addCardButtonVisibility;
            set
            {
                _addCardButtonVisibility = value;
                OnPropertyChange();
            }
        }

        private Visibility _cardInfoVisibility;
        public Visibility CardInfoVisibility
        {
            get => _cardInfoVisibility;
            set
            {
                _cardInfoVisibility = value;
                OnPropertyChange();
            }
        }

        private Visibility _adminUserVisibility;
        public Visibility AdminUserVisibility
        {
            get => _adminUserVisibility;
            set
            {
                _adminUserVisibility = value;
                OnPropertyChange();
            }
        }



        // User
        public string Username
        {
            get => _loggedInUser.Data.Username;
            set
            {
                _loggedInUser.Data.Username = value;
                OnPropertyChange();
            }
        }

        public string Role
        {
            get => _loggedInUser.Data.Role;
            set
            {
                _loggedInUser.Data.Role = value;
                OnPropertyChange();
            }
        }

        public string Email
        {
            get => _loggedInUser.Data.Email;
            set
            {
                _loggedInUser.Data.Email = value;
                OnPropertyChange();
            }
        }

        public string FirstName
        {
            get => _loggedInUser.Data.FirstName;
            set
            {
                _loggedInUser.Data.FirstName = value;
                OnPropertyChange();
            }
        }

        public string LastName
        {
            get => _loggedInUser.Data.LastName;
            set
            {
                _loggedInUser.Data.LastName = value;
                OnPropertyChange();
            }
        }

        public string Gender
        {
            get => _loggedInUser.Data.Gender;
            set
            {
                _loggedInUser.Data.Gender = value;
                OnPropertyChange();
            }
        }

        public string PhoneNumber
        {
            get => _loggedInUser.Data.PhoneNumber;
            set
            {
                _loggedInUser.Data.PhoneNumber = value;
                OnPropertyChange();
            }
        }

        public string Address
        {
            get => _loggedInUser.Data.Address;
            set
            {
                _loggedInUser.Data.Address = value;
                OnPropertyChange();
            }
        }


        // Bank detail
        public string CardNumber
        {
            get => _bankDetailData.CardNumber;
            set
            {
                _bankDetailData.CardNumber = value;
                OnPropertyChange();
            }
        }

        public string ExpirationDate
        {
            get => _bankDetailData.ExpirationDate;
            set
            {
                _bankDetailData.ExpirationDate = value;
                OnPropertyChange();
            }
        }

        public string CardCVV
        {
            get => _bankDetailData.CardCVV;
            set
            {
                _bankDetailData.CardCVV = value;
                OnPropertyChange();
            }
        }

        public string CardholderName
        {
            get => _bankDetailData.CardHolderName;
            set
            {
                _bankDetailData.CardHolderName = value;
                OnPropertyChange();
            }
        }

        public string BillingAddress
        {
            get => _bankDetailData.BillingAddress;
            set
            {
                _bankDetailData.BillingAddress = value;
                OnPropertyChange();
            }
        }

        public RelayCommand LogoutCommand => new RelayCommand(Logout);
        public RelayCommand EditOrUpdateCommand => new RelayCommand(EditOrUpdate);
        public RelayCommand AddCardCommand => new RelayCommand(AddCard, (o) => !_isNotEditing);
        public RelayCommand UploadPictureCommand => new RelayCommand(UploadPicture, (o) => !_isNotEditing);
        public RelayCommand AdminPanelCommand => new RelayCommand(AdminPanel);

        public UserViewModel(MainViewModel mainViewModel)
        {
            _mainViewModel = mainViewModel;
            _loggedInUser = _mainViewModel.TradingCompany.LoggedInUser;

            if (_loggedInUser == null)
                throw new InvalidOperationException("Not logged in");

            _bankDetailData = _loggedInUser.BankDetailData == null 
                ? new BankDetailData()
                : _loggedInUser.BankDetailData;


            IsNotEditing = true;
            EditOrUpdateContent = "Edit";

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

            DisplayBankCreditDetail(_mainViewModel.TradingCompany.LoggedInUser);
            DisplayImage(userData);

            AdminUserVisibility = _mainViewModel.TradingCompany.LoggedInUser.Role == UserRole.Admin
                ? Visibility.Visible
                : Visibility.Collapsed;
        }

        private void DisplayBankCreditDetail(User user)
        {
            if (user.BankDetailData == null)
            {
                CardInfoVisibility = Visibility.Collapsed;
                AddCardButtonVisibility = Visibility.Visible;
                return;
            }

            CardNumber = user.BankDetailData.CardNumber;
            ExpirationDate = user.BankDetailData.ExpirationDate;
            CardCVV = user.BankDetailData.CardCVV;
            CardholderName = user.BankDetailData.CardHolderName;
            BillingAddress = user.BankDetailData.BillingAddress;

            AddCardButtonVisibility = Visibility.Collapsed;
        }

        private void DisplayImage(UserData userData)
        {
            if (userData.ProfilePicture == null)
                return;

            var image = LoadImageFromBytes(userData.ProfilePicture);
            if (image == null)
                return;

            UserProfile = image;
        }

        private void Logout(object? o)
        {
            // TODO:
            _mainViewModel.TradingCompany.LogOut();
            _mainViewModel.Navigate(MainViewModel.Pages.Login);
        }

        private void EditOrUpdate(object? o)
        {
            User? user = _mainViewModel.TradingCompany.LoggedInUser;
            if (user == null) 
                return;

            IsNotEditing = !IsNotEditing;

            if (!IsNotEditing)
            {
                EditOrUpdateContent = "Update";
            }
            else
            {
                ValidateBankCard(user);

                EditOrUpdateContent = "Edit";
                _mainViewModel.TradingCompany.UpdateUser(user);
            }

            OnPropertyChange(nameof(AddCardCommand));
            OnPropertyChange(nameof(UploadPictureCommand));
        }

        private void ValidateBankCard(User user)
        {
            if (AddCardButtonVisibility == Visibility.Visible)
            {
                return;
            }

            if (!BankDetailData.IsValidCardNumber(_bankDetailData.CardNumber))
            {
                MessageBox.Show("Invalid card number");
                IsNotEditing = false;
                return;
            }

            if (!BankDetailData.IsValidExpirationDate(_bankDetailData.ExpirationDate))
            {
                MessageBox.Show("Invalid expiration date");
                IsNotEditing = false;
                return;
            }

            if (!BankDetailData.IsValidCVV(_bankDetailData.CardCVV))
            {
                MessageBox.Show("Invalid CVV");
                IsNotEditing = false;
                return;
            }

            _bankDetailData.UserId = user.Data.UserId;
            user.BankDetailData = _bankDetailData;
        }
        

        private void UploadPicture(object? o)
        {
            var dialog = new Microsoft.Win32.OpenFileDialog();
            dialog.DefaultExt = ".png";
            dialog.Filter = "Images (.png)|*.png";

            bool? result = dialog.ShowDialog();

            if (result != true)
            {
                return;
            }

            try
            {
                string filename = dialog.FileName;
                byte[] bytes = File.ReadAllBytes(filename);

                UserProfile = LoadImageFromBytes(bytes);
                _loggedInUser.Data.ProfilePicture = bytes;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error reading the file: {ex.Message}");
            }
        }

        private void AddCard(object? o)
        {
            AddCardButtonVisibility = Visibility.Collapsed;
            CardInfoVisibility = Visibility.Visible;
        }

        private void AdminPanel(object? o)
        {
            _mainViewModel.Navigate(MainViewModel.Pages.AdminPanel);
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
