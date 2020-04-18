using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Cook_Book_Mobile.Helpers
{
   public static class TempData
    {
        public static string GetTempFolderPath()
        {
            try
            {
                return Path.GetTempPath();
               // string directory = Path.Combine(Android.OS.Environment.ExternalStorageDirectory.AbsolutePath, Android.OS.Environment.DirectoryDownloads);
               // return directory;
            }
            catch (Exception ex)
            {
                //_logger.Error("Got exception", ex);
                throw;
            }
        }

        public static string GetImagePath(string name)
        {
            string path = "";
            try
            {
                path = GetTempFolderPath() + $@"{name}";
                return path;
            }
            catch (Exception ex)
            {
                //_logger.Error("Got exception", ex);
                throw;
            }
        }

        public static bool ImageExistOnDisk(string name)
        {
            bool output = false;

            try
            {
                if (File.Exists(GetImagePath(name)))
                {
                    output = true;
                }

            }
            catch (Exception ex)
            {
                //_logger.Error("Got exception", ex);
                throw;
            }

            return output;
        }

        public static void DeleteUnusedImages(List<string> dontDeletetheseImages)
        {
            List<string> ImagesInFolder = new List<string>();

            try
            {
                string[] fileArray = Directory.GetFiles(Path.GetTempPath());

                foreach (var item in fileArray)
                {
                    ImagesInFolder.Add(Path.GetFileName(item));
                }

                foreach (var item in dontDeletetheseImages)
                {
                    ImagesInFolder.RemoveAll(x => x == item);
                }

                foreach (var item in ImagesInFolder)
                {
                    File.Delete(GetImagePath(item));
                }

            }
            catch (IOException ex)
            {
                //Spodziewany błąd. Przy kolejnym uruchomieniu aplikacji zasoby będa odblokowane i problem nie wystąpi
               // _logger.Warn("Cannot delete images", ex);
            }

            catch (Exception ex)
            {
                //_logger.Error("Got exception", ex);
                throw;
            }
        }
    }
}
