using Cook_Book_Mobile.Helpers;
using Cook_Book_Mobile.ViewModels;
using Cook_Book_Shared_Code.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
                MessagingCenter.Send(this, EventMessages.ReloadRecipesEvent);
            });

            MessagingCenter.Subscribe<RecipePreviewViewModel, RecipeModel>(this, EventMessages.EditRecipeEvent, async (sender, arg) =>
            {
                await Navigation.PushAsync(new AddOrEditPage(),true);
                MessagingCenter.Send(this, EventMessages.EditRecipeEvent, arg);
            });

        }
    }
}