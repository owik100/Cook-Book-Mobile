using Cook_Book_Mobile.Helpers;
using Cook_Book_Mobile.ViewModels;

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

            MessagingCenter.Subscribe<AddOrEditViewModel>(this, EventMessages.BasicNavigationEvent, async (sender) =>
            {
                await Navigation.PopToRootAsync(true);
                MessagingCenter.Send(this, EventMessages.BasicNavigationEvent);
            });
        }
    }
}