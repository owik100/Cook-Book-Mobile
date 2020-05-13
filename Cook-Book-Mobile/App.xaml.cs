using Cook_Book_Mobile.Helpers;
using Cook_Book_Mobile.Services;
using Cook_Book_Mobile.Views;
using Xamarin.Forms;

namespace Cook_Book_Mobile
{
    public partial class App : Application
    {

        public App()
        {
            InitializeComponent();

            DependencyService.Register<MockDataStore>();
            DependencyService.Register<IMessage>();

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
