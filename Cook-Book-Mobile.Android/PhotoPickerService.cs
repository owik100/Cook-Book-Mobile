using Android.Content;
using Cook_Book_Mobile.Droid;
using Cook_Book_Mobile.Services;
using System.Threading.Tasks;
using Xamarin.Forms;

[assembly: Dependency(typeof(PhotoPickerService))]
namespace Cook_Book_Mobile.Droid
{
    public class PhotoPickerService : IPhotoPickerService
    {
        public Task<string> GetImagePathAsync()
        {
            // Define the Intent for getting images
            Intent intent = new Intent();
            intent.SetType("image/*");
            intent.SetAction(Intent.ActionGetContent);

            // Start the picture-picker activity (resumes in MainActivity.cs)
            MainActivity.Instance.StartActivityForResult(
                Intent.CreateChooser(intent, "Select Picture"),
                MainActivity.PickImageId);

            // Save the TaskCompletionSource object as a MainActivity property
            MainActivity.Instance.PickImageTaskCompletionSource = new TaskCompletionSource<string>();

            // Return Task object
            return MainActivity.Instance.PickImageTaskCompletionSource.Task;
        }

        //public Task<Stream> GetImageStreamAsync()
        //{
        //    // Define the Intent for getting images
        //    Intent intent = new Intent();
        //    intent.SetType("image/*");
        //    intent.SetAction(Intent.ActionGetContent);

        //    // Start the picture-picker activity (resumes in MainActivity.cs)
        //    MainActivity.Instance.StartActivityForResult(
        //        Intent.CreateChooser(intent, "Select Picture"),
        //        MainActivity.PickImageId);

        //    // Save the TaskCompletionSource object as a MainActivity property
        //    MainActivity.Instance.PickImageTaskCompletionSource = new TaskCompletionSource<Stream>();

        //    // Return Task object
        //    return MainActivity.Instance.PickImageTaskCompletionSource.Task;
        //}
    }
}