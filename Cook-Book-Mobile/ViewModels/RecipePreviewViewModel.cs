using Cook_Book_Mobile.Helpers;
using Cook_Book_Mobile.Views;
using Cook_Book_Shared_Code.Models;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace Cook_Book_Mobile.ViewModels
{
    public class RecipePreviewViewModel : BaseViewModel
    {
        public RecipePreviewViewModel()
        {
            MessagingCenter.Subscribe<RecipesPage, RecipeModel>(this, EventMessages.RecipesPreviewEvent, (sender, arg) =>
            {
              
            });
        }
    }
}
