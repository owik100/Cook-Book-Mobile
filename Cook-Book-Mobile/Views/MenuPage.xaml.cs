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
        public MenuPage()
        {
            InitializeComponent();

            ListViewMenu.ItemSelected += async (sender, e) =>
            {
                if (e.SelectedItem == null)
                    return;

                var id = (int)((HomeMenuItem)e.SelectedItem).Id;
                await RootPage.NavigateFromMenu(id);
            };
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();

          
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

         
        }
    }
}