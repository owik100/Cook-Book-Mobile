using Cook_Book_Mobile.Helpers;
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

            MessagingService.Current.Unsubscribe(EventMessages.RecipePreviewViewModelPage);

            MessagingService.Current.Subscribe(EventMessages.RecipePreviewViewModelPage, async (sender) =>
            {
                await Navigation.PopAsync(true);
                MessagingService.Current.SendMessage(EventMessages.ReloadUserRecipesEvent);
            });

            MessagingService.Current.Unsubscribe<RecipeModel>(EventMessages.EditRecipeEvent);

            MessagingService.Current.Subscribe<RecipeModel>(EventMessages.EditRecipeEvent, async (sender, arg) => 
            {
                await Navigation.PushAsync(new AddOrEditPage(), true);
                MessagingService.Current.SendMessage(EventMessages.EditRecipeEvent, arg);
            });

        }
    }
}