using Cook_Book_Mobile.Helpers;
using Cook_Book_Mobile.ViewModels;
using FormsToolkit;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Cook_Book_Mobile.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AddOrEditPage : ContentPage
    {
        public AddOrEditPage()
        {
            InitializeComponent();

            MessagingService.Current.Unsubscribe(EventMessages.AddOrEditViewModelPage);

            MessagingService.Current.Subscribe(EventMessages.AddOrEditViewModelPage, async (sender) =>
            {
                await Navigation.PopToRootAsync(true);
                MessagingService.Current.SendMessage(EventMessages.ReloadUserRecipesEvent);
            });

        }
    }
}