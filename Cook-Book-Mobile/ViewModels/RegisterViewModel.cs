using Cook_Book_Shared_Code.API;
using Cook_Book_Shared_Code.Models;
using System;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace Cook_Book_Mobile.ViewModels
{
    public class RegisterViewModel : BaseViewModel
    {
        private string _userName;
        private string _password;
        private string _passwordRepeat;
        private string _email;

        public ICommand InfoCommand { get; set; }

        private IAPIHelper _apiHelper;

        public RegisterViewModel(IAPIHelper APIHelper)
        {
            Title = "Rejestracja";
            InfoCommand = new Command(async () => await Register());

            _apiHelper = APIHelper;

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
                OnPropertyChanged(nameof(CanRegister));
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
                OnPropertyChanged(nameof(CanRegister));
            }
        }

        public string PasswordRepeat
        {
            get { return _passwordRepeat; }
            set
            {
                _passwordRepeat = value;
                //SetProperty(ref _passwordRepeat, value);
                OnPropertyChanged(nameof(PasswordRepeat));
                OnPropertyChanged(nameof(CanRegister));
            }
        }

        public string Email
        {
            get { return _email; }
            set
            {
                _email = value;
                //SetProperty(ref _email, value);
                OnPropertyChanged(nameof(Email));
                OnPropertyChanged(nameof(CanRegister));
            }
        }


        public bool CanRegister
        {
            get
            {
                bool output = false;

                if (UserName?.Length > 0 && Email?.Length > 0 && Password?.Length > 0 && PasswordRepeat?.Length > 0 && !IsBusy)
                {
                    output = true;
                }

                return output;
            }

        }

        #endregion

        private bool CheckSamePasswords()
        {
            bool output = false;

            if (Password.Equals(PasswordRepeat))
            {
                output = true;
            }

            return output;
        }


        private async Task Register()
        {
            try
            {
                if (!CheckSamePasswords())
                {
                    await Application.Current.MainPage.DisplayAlert("Błąd!", "Hasła nie są identyczne", "Ok");
                    return;
                }

                IsBusy = true;
                OnPropertyChanged(nameof(CanRegister));

                RegisterModel user = new RegisterModel
                {
                    UserName = UserName,
                    Email = Email,
                    Password = Password,
                    ConfirmPassword = PasswordRepeat
                };

                var result = await _apiHelper.Register(user);

                await Application.Current.MainPage.DisplayAlert("Sukces", "Rejestracja pomyślna. Możesz się teraz zalogować", "Ok");

                Clear();
            }
            catch (Exception ex)
            {
                //  _logger.Error("Got exception", ex);
                await Application.Current.MainPage.DisplayAlert("Błąd", ex.Message, "Ok");

            }
            finally
            {
                IsBusy = false;
                OnPropertyChanged(nameof(CanRegister));
            }
        }

        private void Clear()
        {
            UserName = string.Empty;
            Password = string.Empty;
            Email = string.Empty;
            PasswordRepeat = string.Empty;
        }
    }
}
