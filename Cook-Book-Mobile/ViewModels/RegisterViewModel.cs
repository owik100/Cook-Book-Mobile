using Cook_Book_Client_Desktop_Library.API;
using Cook_Book_Client_Desktop_Library.API.Interfaces;
using Cook_Book_Client_Desktop_Library.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
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

        private IAPIHelper _apiHelper;

        //public RegisterViewModel(IAPIHelper apiHelper)
        public RegisterViewModel()
        {
            Title = "Rejestracja";
            _apiHelper = new APIHelper(new LoggedUser());
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

        public Command InfoCommand
        {
            get
            {
                return new Command(async () =>
                {
                    await Application.Current.MainPage.DisplayAlert("Informacja", UserName + " " + Password + " " + PasswordRepeat + " " + Email, "Ok");

                    try
                    {
                        _duringOperation = true;
                        // NotifyOfPropertyChange(() => CanRegister);

                        RegisterModel user = new RegisterModel
                        {
                            UserName = UserName,
                            Email = Email,
                            Password = Password,
                            ConfirmPassword = PasswordRepeat
                        };

                        var result = await _apiHelper.Register(user);
                        //RegisterInfoMessage = "Rejestracja pomyślna. Możesz się teraz zalogować";

                        Clear();
                        _duringOperation = false;
                        // NotifyOfPropertyChange(() => CanRegister);
                    }
                    catch (Exception ex)
                    {
                        _duringOperation = false;
                        //  NotifyOfPropertyChange(() => CanRegister);
                        //  _logger.Error("Got exception", ex);
                        //  RegisterInfoMessage = ex.Message;
                    }

                });
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
