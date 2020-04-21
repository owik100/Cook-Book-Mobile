using Cook_Book_Mobile.Helpers;
using Cook_Book_Mobile.ViewModels;
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
    public partial class AddOrEditPage : ContentPage
    {
        public AddOrEditPage()
        {
            InitializeComponent();

            MessagingCenter.Subscribe<AddOrEditViewModel>(this, EventMessages.BasicNavigationEvent, async (sender) =>
            {
                await Navigation.PopToRootAsync(true);
                MessagingCenter.Send(this, EventMessages.ReloadUserRecipesEvent);
            });
        }
    }
}