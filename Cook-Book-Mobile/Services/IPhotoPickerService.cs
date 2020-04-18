using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace Cook_Book_Mobile.Services
{
    public interface IPhotoPickerService
    {
        Task<string> GetImagePathAsync();
    }
}
