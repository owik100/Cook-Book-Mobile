using Cook_Book_Mobile.Models;
using Cook_Book_Mobile.Views;
using Cook_Book_Shared_Code.API;
using Cook_Book_Shared_Code.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace Cook_Book_Mobile.ViewModels
{
    public class RecipesViewModel : BaseViewModel
    {
        public ICommand LogOutCommand { get; set; }

        private ILoggedUser _loggedUser;
        private IAPIHelper _apiHelper;

        public RecipesViewModel(ILoggedUser loggedUser, IAPIHelper helper)
        {
            _loggedUser = loggedUser;
            _apiHelper = helper;

            LogOutCommand = new Command(async ()  => await LogOut());

        }
        private async Task LogOut()
        {
            _loggedUser.LogOffUser();
            _apiHelper.LogOffUser();
            SecureStorage.RemoveAll();

            await (Application.Current.MainPage as MainPage).NavigateFromMenu((int)MenuItemType.Login);
        }
    }
}
