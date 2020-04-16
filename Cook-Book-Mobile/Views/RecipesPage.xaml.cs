using Cook_Book_Mobile.Helpers;
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
    public partial class RecipesPage : ContentPage
    {
        public RecipesPage()
        {
            InitializeComponent();
        }

        async void OnItemSelected(object sender, SelectedItemChangedEventArgs args)
        {

            var item = args.SelectedItem as RecipeModel;
            if (item == null)
                return;

            await Navigation.PushAsync(new RecipePreviewPage());

            ListViewRecipes.SelectedItem = null;

            MessagingCenter.Send(this, EventMessages.RecipesPreviewEvent, item);
        }

        private async void AddRecipe_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new AddOrEditPage(), true);
           // MessagingCenter.Send(this, EventMessages.AddOrEditEvent);
        }
    }
}