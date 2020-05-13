using Cook_Book_Mobile.Helpers;
using Cook_Book_Mobile.Models;
using Cook_Book_Shared_Code.Models;
using System;
using System.Linq;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Cook_Book_Mobile.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class RecipesPage : ContentPage
    {
        public RecipesPage()
        {
            InitializeComponent();
        }

        async void OnCollectionViewSelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            var item = e.CurrentSelection.FirstOrDefault() as RecipeModelDisplay;
            if (item == null)
                return;

            await Navigation.PushAsync(new RecipePreviewPage());

            ListViewRecipes.SelectedItem = null;

            RecipeAndTitlePage recipeAndTitlePage = new RecipeAndTitlePage(item, Title);

            MessagingCenter.Send(this, EventMessages.RecipesPreviewEvent, recipeAndTitlePage);
        }

        private async void AddRecipe_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new AddOrEditPage(), true);
        }

    }
}