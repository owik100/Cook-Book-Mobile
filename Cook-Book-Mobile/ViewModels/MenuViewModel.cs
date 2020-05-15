using Cook_Book_Mobile.Helpers;
using Cook_Book_Mobile.Models;
using Cook_Book_Mobile.Views;
using Cook_Book_Shared_Code.API;
using Cook_Book_Shared_Code.Models;
using FormsToolkit;
using System;
using System.Collections.ObjectModel;
using System.Linq;
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

        static bool once;

        public MenuViewModel(ILoggedUser loggedUser, IAPIHelper apiHelper)
        {
            _loggedUser = loggedUser;
            _apiHelper = apiHelper;
            LogOutCommand = new Command(() => LogOut());

                NotLoggedMenu();

                SelectedItem = MenuItems.Where(x => x.Id == MenuItemType.Login).FirstOrDefault();

            MessagingService.Current.Unsubscribe(EventMessages.BasicNavigationEvent);
            MessagingService.Current.Subscribe(EventMessages.BasicNavigationEvent, (sender) =>
            {
                SelectedItem = MenuItems.Where(x => x.Id == MenuItemType.UserRecipes).FirstOrDefault();
                //Wywola sie event w code behind, przekierowujacy do wybranej strony
            });

            MessagingService.Current.Unsubscribe(EventMessages.LogOnEvent);
            MessagingService.Current.Subscribe( EventMessages.LogOnEvent, (sender) =>
            {
                OnPropertyChanged(nameof(Logged));
                LoggedMenu();
                SelectedItem = MenuItems.Where(x => x.Id == MenuItemType.UserRecipes).FirstOrDefault();

                //MessagingCenter.Send(this, EventMessages.ReloadUserRecipesEvent);
            });

            MessagingService.Current.Unsubscribe<MenuItemType>(EventMessages.BasicNavigationEvent);
            MessagingService.Current.Subscribe<MenuItemType>(EventMessages.BasicNavigationEvent, (sender, arg) =>
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

                if (SelectedItem == MenuItems.Where(x => x.Id == MenuItemType.PublicRecipes).FirstOrDefault())
                {
                    MessagingService.Current.SendMessage(EventMessages.ReloadPublicRecipesEvent);
                }

                if (SelectedItem == MenuItems.Where(x => x.Id == MenuItemType.UserRecipes).FirstOrDefault())
                {
                    MessagingService.Current.SendMessage(EventMessages.ReloadUserRecipesEvent);
                }

                if (SelectedItem == MenuItems.Where(x => x.Id == MenuItemType.FavouritesRecipes).FirstOrDefault())
                {
                    MessagingService.Current.SendMessage(EventMessages.ReloadFavouritesRecipesEvent);
                }
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
                new HomeMenuItem {Id = MenuItemType.About, Title="O aplikacji" },
            };
            OnPropertyChanged(nameof(MenuItems));
        }

        private void LoggedMenu()
        {
            MenuItems = new ObservableCollection<HomeMenuItem>
            {
                new HomeMenuItem {Id = MenuItemType.UserRecipes, Title="Moje przepisy" },
                new HomeMenuItem {Id = MenuItemType.PublicRecipes, Title="Odkrywaj przepisy" },
                new HomeMenuItem {Id = MenuItemType.FavouritesRecipes, Title="Ulubione przepisy" },
                new HomeMenuItem {Id = MenuItemType.About, Title="O aplikacji" },
            };
            OnPropertyChanged(nameof(MenuItems));
        }

        private void LogOut()
        {
            _loggedUser.LogOffUser();
            _apiHelper.LogOffUser();
            SecureStorage.RemoveAll();
            MessagingService.Current.SendMessage(EventMessages.LogOffEvent);

            OnPropertyChanged(nameof(Logged));

            NotLoggedMenu();
            SelectedItem = MenuItems.Where(x => x.Id == MenuItemType.Login).FirstOrDefault();
        }
    }
}
