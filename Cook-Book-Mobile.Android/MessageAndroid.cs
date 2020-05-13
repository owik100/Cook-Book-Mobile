
using Android.App;
using Android.Widget;
using Cook_Book_Mobile.Droid;
using Cook_Book_Mobile.Helpers;

[assembly: Xamarin.Forms.Dependency(typeof(MessageAndroid))]
namespace Cook_Book_Mobile.Droid
{
    public class MessageAndroid : IMessage
    {
        public void LongAlert(string message)
        {
            Toast.MakeText(Application.Context, message, ToastLength.Long).Show();
        }

        public void ShortAlert(string message)
        {
            Toast.MakeText(Application.Context, message, ToastLength.Short).Show();
        }
    }
}