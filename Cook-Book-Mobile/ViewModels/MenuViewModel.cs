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
        public ICommand LogOutCommand { get; set; }

        private ObservableCollection<HomeMenuItem> _menuItems;
        private HomeMenuItem _selectedItem;
        private ILoggedUser _loggedUser;
        private IAPIHelper _apiHelper;
        private string _helloText;

        public MenuViewModel(ILoggedUser loggedUser, IAPIHelper apiHelper)
        {
            _loggedUser = loggedUser;
            _apiHelper = apiHelper;
            LogOutCommand = new Command(() => LogOut());

            NotLoggedMenu();

            SelectedItem = MenuItems.Where(x => x.Id == MenuItemType.Login).FirstOrDefault();

            MessagingCenter.Subscribe<LoginViewModel>(this, EventMessages.LogOnEvent, (sender) =>
            {
                OnPropertyChanged(nameof(Logged));
                LoggedMenu();
                SelectedItem = MenuItems.Where(x => x.Id == MenuItemType.Recipes).FirstOrDefault();

                MessagingCenter.Send(this, EventMessages.ReloadRecipesEvent);
            });

            MessagingCenter.Subscribe<LoginViewModel, MenuItemType>(this, EventMessages.NavigationEvent, (sender, arg) =>
            {
                SelectedItem = MenuItems.Where(x => x.Id == arg).FirstOrDefault();
                //Wywola sie event w code behind, przekierowujacy do wybranej strony
            });
        }

        #region props

        public string HelloText
        {
            get
            {
                return _helloText;
            }
            set
            {
                _helloText = "Witaj " + value;
                //SetProperty(ref _helloText, value);
                OnPropertyChanged(nameof(HelloText));
            }
        }

        public ObservableCollection<HomeMenuItem> MenuItems
        {
            get
            {
                return _menuItems;
            }
            set
            {
                _menuItems = value;
                OnPropertyChanged(nameof(MenuItems));
            }
        }

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

        public bool Logged
        {
            get
            {
                bool output = false;

                if (string.IsNullOrWhiteSpace(_loggedUser.Token) == false)
                {
                    output = true;
                    HelloText = _loggedUser.UserName;             
                }
                else
                {
                    HelloText = "";
                }
                return output;
            }
        }


        #endregion

        private void NotLoggedMenu()
        {
            MenuItems = new ObservableCollection<HomeMenuItem>
            {
                new HomeMenuItem {Id = MenuItemType.Login, Title="Logowanie" },
                new HomeMenuItem {Id = MenuItemType.Register, Title="Rejestracja" },
                new HomeMenuItem {Id = MenuItemType.About, Title="About" },
            };
            OnPropertyChanged(nameof(MenuItems));
        }

        private void LoggedMenu()
        {
            MenuItems = new ObservableCollection<HomeMenuItem>
            {
                new HomeMenuItem {Id = MenuItemType.Recipes, Title="Przepisy" },
                new HomeMenuItem {Id = MenuItemType.AddRecipe, Title="Dodaj przepis" },
                new HomeMenuItem {Id = MenuItemType.About, Title="About" },
            };
            OnPropertyChanged(nameof(MenuItems));
        }

        private void LogOut()
        {
            _loggedUser.LogOffUser();
            _apiHelper.LogOffUser();
            SecureStorage.RemoveAll();
            OnPropertyChanged(nameof(Logged));

            NotLoggedMenu();
            SelectedItem = MenuItems.Where(x => x.Id == MenuItemType.Login).FirstOrDefault();
        }
    }
}
