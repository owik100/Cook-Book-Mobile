﻿using Cook_Book_Mobile.Helpers;
using Cook_Book_Mobile.ViewModels;
using Cook_Book_Shared_Code.Models;
using FormsToolkit;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Cook_Book_Mobile.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class RecipePreviewPage : ContentPage
    {
        public RecipePreviewPage()
        {
            InitializeComponent();

            MessagingCenter.Subscribe<RecipePreviewViewModel>(this, EventMessages.BasicNavigationEvent, async (sender) =>
            {
                await Navigation.PopAsync(true);
                MessagingCenter.Send(this, EventMessages.ReloadUserRecipesEvent);
            });

            //MessagingCenter.Subscribe<RecipePreviewViewModel, RecipeModel>(this, EventMessages.EditRecipeEvent, async (sender, arg) =>
            //{
            //    await Navigation.PushAsync(new AddOrEditPage(), true);
            //    MessagingCenter.Send(this, EventMessages.EditRecipeEvent, arg);
            //});


            //MessagingService.Current.Unsubscribe(EventMessages.BasicNavigationEvent);

            //MessagingService.Current.Subscribe(EventMessages.BasicNavigationEvent, async (sender) => 
            //{
            //    await Navigation.PopAsync(true);
            //    MessagingCenter.Send(this, EventMessages.ReloadUserRecipesEvent);
            //});



            MessagingService.Current.Unsubscribe<RecipeModel>(EventMessages.EditRecipeEvent);

            MessagingService.Current.Subscribe<RecipeModel>(EventMessages.EditRecipeEvent, async (sender, arg) => 
            {
                await Navigation.PushAsync(new AddOrEditPage(), true);
                MessagingService.Current.SendMessage(EventMessages.EditRecipeEvent, arg);
            });

        }
    }
}