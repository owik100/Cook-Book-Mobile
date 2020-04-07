using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Cook_Book_Mobile.Services;
using Cook_Book_Mobile.Views;

namespace Cook_Book_Mobile
{
    public partial class App : Application
    {

        public App()
        {
            InitializeComponent();

            DependencyService.Register<MockDataStore>();
            MainPage = new MainPage();
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
