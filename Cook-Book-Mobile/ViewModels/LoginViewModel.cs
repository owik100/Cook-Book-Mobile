using Cook_Book_Mobile.API;
using Cook_Book_Mobile.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Essentials;
using Cook_Book_Mobile.Views;
using Cook_Book_Shared_Code.API;
using Cook_Book_Shared_Code.Models;
using Cook_Book_Mobile.Helpers;

namespace Cook_Book_Mobile.ViewModels
{
    public class LoginViewModel : BaseViewModel
    {
        private string _userName;
        private string _password;
        private bool _remember;

        public ICommand InfoCommand { get; set; }
        public ICommand RegisterCommand { get; set; }

        private IAPIHelper _apiHelper;

        public LoginViewModel(IAPIHelper APIHelper)
        {
            Title = "Logowanie";
            InfoCommand = new Command(async () => await Login());
            RegisterCommand = new Command(() => GoRegister());

            _apiHelper = APIHelper;

            MessagingCenter.Subscribe<App>(this, EventMessages.AppStartEvent, async (sender) =>
            {
                await TryLoginOnStart();
            });
        }

        #region Props
        public string UserName
        {
            get { return _userName; }
            set
            {
                _userName = value;
                //SetProperty(ref _userName, value);
                OnPropertyChanged(nameof(UserName));
                OnPropertyChanged(nameof(CanLogin));             
            }
        }

        public string Password
        {
            get { return _password; }
            set
            {
                _password = value;
                //SetProperty(ref _password, value);
                OnPropertyChanged(nameof(Password));
                OnPropertyChanged(nameof(CanLogin));
            }
        }

        public bool Remember
        {
            get { return _remember; }
            set
            {
                _remember = value;
                //SetProperty(ref _remember, value);
                OnPropertyChanged(nameof(Remember));
            }
        }

        public bool CanLogin
        {
            get
            {
                bool output = false;

                if (UserName?.Length > 0 && Password?.Length > 0 && !IsBusy)
                {
                    output = true;
                }

                return output;
            }

        }

        #endregion


        private async Task Login()
        {
            try
            {

                IsBusy = true;
                OnPropertyChanged(nameof(CanLogin));

                AuthenticatedUser user = await _apiHelper.Authenticate(UserName, Password);

                await _apiHelper.GetLoggedUserData(user.Access_Token);

                if (Remember)
                {
                   await SaveUserLoginPassword();
                }
                else
                {
                    SecureStorage.RemoveAll();
                }

                Clear();
                MessagingCenter.Send(this, EventMessages.LogOnEvent);
                await (Application.Current.MainPage as MainPage).NavigateFromMenu((int)MenuItemType.Recipes);
            }
            catch (Exception ex)
            {
                //  _logger.Error("Got exception", ex);
                await Application.Current.MainPage.DisplayAlert("Błąd", ex.Message, "Ok");
            }
            finally
            {
                IsBusy = false;
                OnPropertyChanged(nameof(CanLogin));
            }
        }

        private void GoRegister()
        {
            MessagingCenter.Send(this, EventMessages.BasicNavigationEvent, MenuItemType.Register);
        }

        private async Task TryLoginOnStart()
        {
            try
            {
                UserName = await SecureStorage.GetAsync("userLogin");
                Password = await SecureStorage.GetAsync("userPassword");

                if (UserName?.Length > 0 && Password?.Length > 0)
                {
                    Remember = true;

                    await Login();
                }
            }
            catch (Exception ex)
            {
                // Possible that device doesn't support secure storage on device.
            }
        }

        private void Clear()
        {
            UserName = string.Empty;
            Password = string.Empty;
        }

        private async Task SaveUserLoginPassword()
        {
            try
            {
                await SecureStorage.SetAsync("userLogin", UserName);
                await SecureStorage.SetAsync("userPassword", Password);
            }
            catch (Exception ex)
            {
                // Possible that device doesn't support secure storage on device.
            }
        }



    }
}

