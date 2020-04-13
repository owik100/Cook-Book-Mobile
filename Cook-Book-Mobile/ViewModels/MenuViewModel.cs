using Cook_Book_Mobile.Helpers;
using Cook_Book_Mobile.Models;
using Cook_Book_Mobile.Views;
using Cook_Book_Shared_Code.API;
using Cook_Book_Shared_Code.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace Cook_Book_Mobile.ViewModels
{
    public class MenuViewModel : BaseViewModel
    {
        public ObservableCollection<HomeMenuItem> MenuItems { get; set; }   
        public ICommand LogOutCommand { get; set; }
        MainPage RootPage { get => Application.Current.MainPage as MainPage; }

        HomeMenuItem _selectedItem;
        public HomeMenuItem SelectedItem
        {
            get
            {
                return _selectedItem;
            }
            set
            {
                _selectedItem = value;
                //SetProperty(ref _selectedItem, value);
                OnPropertyChanged(nameof(SelectedItem));
            }
        }


        private ILoggedUser _loggedUser;
        private IAPIHelper _apiHelper;

        bool _logged;
        public bool Logged
        {
            get
            {
                bool output = false;

                if (string.IsNullOrWhiteSpace(_loggedUser.Token) == false)
                {
                    output = true;
                }

                return output;
            }
        }

        public MenuViewModel(ILoggedUser loggedUser, IAPIHelper apiHelper)
        {
            _loggedUser = loggedUser;
            _apiHelper = apiHelper;
            LogOutCommand = new Command(async () => await LogOut());

            MenuItems = new ObservableCollection<HomeMenuItem>
            {
                new HomeMenuItem {Id = MenuItemType.Login, Title="Logowanie" },
                new HomeMenuItem {Id = MenuItemType.Register, Title="Rejestracja" },
                new HomeMenuItem {Id = MenuItemType.About, Title="About" },
            };

            SelectedItem = MenuItems[0];

            MessagingCenter.Subscribe<LoginViewModel>(this, EventMessages.LogOnOffEvent, (sender) =>
            {
                OnPropertyChanged(nameof(Logged));
            });

            MessagingCenter.Subscribe<LoginViewModel, MenuItemType>(this, EventMessages.NavigationEvent, (sender, arg) =>
            {
                 SelectedItem = MenuItems.Where(x => x.Id == arg).FirstOrDefault();
            });
        }

        private async Task LogOut()
        {
            _loggedUser.LogOffUser();
            _apiHelper.LogOffUser();
            SecureStorage.RemoveAll();
            OnPropertyChanged(nameof(Logged));

            await (Application.Current.MainPage as MainPage).NavigateFromMenu((int)MenuItemType.Login);
        }
    }
}
