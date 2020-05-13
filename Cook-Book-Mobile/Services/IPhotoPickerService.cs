using System.Threading.Tasks;

namespace Cook_Book_Mobile.Services
{
    public interface IPhotoPickerService
    {
        Task<string> GetImagePathAsync();
    }
}
