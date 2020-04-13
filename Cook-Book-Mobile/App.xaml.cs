using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Cook_Book_Mobile.Services;
using Cook_Book_Mobile.Views;
using Cook_Book_Mobile.Helpers;

namespace Cook_Book_Mobile
{
    public partial class App : Application
    {

        public App()
        {
            InitializeComponent();

            DependencyService.Register<MockDataStore>();
            MainPage = new MainPage();

            MessagingCenter.Send(this, EventMessages.AppStartEvent);
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
