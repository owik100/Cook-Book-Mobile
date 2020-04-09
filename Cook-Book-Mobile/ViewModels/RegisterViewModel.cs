using Cook_Book_Mobile.API;
using Cook_Book_Mobile.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
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

        private bool _duringOperation;

        public ICommand InfoCommand { get; set; }

        APIHelper _APIHelper;

        //public RegisterViewModel(IAPIHelper apiHelper)
        public RegisterViewModel()
        {
            Title = "Rejestracja";
            InfoCommand = new Command(async () => await Register());

            _APIHelper = new APIHelper(new LoggedUser());
        }

        #region Props
        public string UserName
        {
            get { return _userName; }
            set
            {
                _userName = value;
                SetProperty(ref _userName, value);
                //NotifyOfPropertyChange(() => CanRegister);
            }
        }

        public string Password
        {
            get { return _password; }
            set
            {
                _password = value;
                SetProperty(ref _password, value);
                //NotifyOfPropertyChange(() => CanRegister);
            }
        }

        public string PasswordRepeat
        {
            get { return _passwordRepeat; }
            set
            {
                _passwordRepeat = value;
                SetProperty(ref _passwordRepeat, value);
                //NotifyOfPropertyChange(() => CanRegister);
            }
        }

        public string Email
        {
            get { return _email; }
            set
            {
                _email = value;
                SetProperty(ref _email, value);
                //NotifyOfPropertyChange(() => CanRegister);
            }
        }

        #endregion


        private async Task Register()
        {
            //await Application.Current.MainPage.DisplayAlert("Informacja", UserName + " " + Password + " " + PasswordRepeat + " " + Email, "Ok");

            try
            {
                _duringOperation = true;
                // NotifyOfPropertyChange(() => CanRegister);

                RegisterModel user = new RegisterModel
                {
                    UserName = "Arnold",
                    Email = "Wor@wwww.com",
                    Password = "Pwd12345.",
                    ConfirmPassword = "Pwd12345."
                };

                var result = await _APIHelper.Register(user);
              //  RegisterInfoMessage = "Rejestracja pomyślna. Możesz się teraz zalogować";


                Clear();
                _duringOperation = false;
                // NotifyOfPropertyChange(() => CanRegister);
            }
            catch (Exception ex)
            {
                _duringOperation = false;
                //  NotifyOfPropertyChange(() => CanRegister);
                //  _logger.Error("Got exception", ex);
                await Application.Current.MainPage.DisplayAlert("ERROR", ex.Message, "Ok");
            }
        }

        private void Clear()
        {
            UserName = "";
            Password = "";
            Email = "";
            PasswordRepeat = "";
        }



    }
}
