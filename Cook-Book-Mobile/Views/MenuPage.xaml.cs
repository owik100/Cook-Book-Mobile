using Cook_Book_Mobile.Helpers;
using Cook_Book_Mobile.Models;
using Cook_Book_Mobile.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Cook_Book_Mobile.Views
{
    // Learn more about making custom code visible in the Xamarin.Forms previewer
    // by visiting https://aka.ms/xamarinforms-previewer
    [DesignTimeVisible(false)]
    public partial class MenuPage : ContentPage
    {
        MainPage RootPage { get => Application.Current.MainPage as MainPage; }
        static bool redirectToUserRecipes;
        static bool logged;

        public MenuPage()
        {
            InitializeComponent();

            ListViewMenu.ItemSelected += async (sender, e) =>
            {
                if (e.SelectedItem == null)
                    return;

                var id = (int)((HomeMenuItem)e.SelectedItem).Id;

                //Sprawdz czy pozwolic przekierowac do przepisow po zalogowaniu po re opoenie appki
                if(id == 4)
                {
                    logged = true;
                }

                await RootPage.NavigateFromMenu(id);
            };
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();

            if(logged)
            {
                redirectToUserRecipes = true;
            }
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            if(logged && redirectToUserRecipes)
            {
                await RootPage.NavigateFromMenu(4);
            }     
        }
    }
}