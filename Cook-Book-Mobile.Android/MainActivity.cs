﻿using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Android.Runtime;
using Android.Views;
using FormsToolkit.Droid;
using System;
using System.Threading.Tasks;

namespace Cook_Book_Mobile.Droid
{
    [Activity(Label = "Cook Book", Icon = "@mipmap/icon", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        internal static MainActivity Instance { get; private set; }

        public static readonly int PickImageId = 1000;
        public TaskCompletionSource<string> PickImageTaskCompletionSource { set; get; }


        protected override void OnCreate(Bundle savedInstanceState)
        {
            Instance = this;

            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(savedInstanceState);

            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            global::Xamarin.Forms.Forms.Init(this, savedInstanceState);
            Window.SetStatusBarColor(Android.Graphics.Color.Rgb(237, 184, 121));

            Toolkit.Init();

            LoadApplication(new App());
        }

        protected override void OnActivityResult(int requestCode, Result resultCode, Intent intent)
        {
            base.OnActivityResult(requestCode, resultCode, intent);

            if (requestCode == PickImageId)
            {
                if ((resultCode == Result.Ok) && (intent != null))
                {
                    Android.Net.Uri uri = intent.Data;
                    string path = GetPathToImage(uri);
                    //Stream stream = ContentResolver.OpenInputStream(uri);

                    PickImageTaskCompletionSource.SetResult(path);
                }
                else
                {
                    PickImageTaskCompletionSource.SetResult(null);
                }
            }
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }

        private string GetPathToImage(Android.Net.Uri uri)
        {
            string path = null;
            try
            {
                string doc_id = "";
                using (var c1 = ContentResolver.Query(uri, null, null, null, null))
                {
                    c1.MoveToFirst();
                    string document_id = c1.GetString(0);
                    doc_id = document_id.Substring(document_id.LastIndexOf(":") + 1);
                }



                // The projection contains the columns we want to return in our query.
                string selection = Android.Provider.MediaStore.Images.Media.InterfaceConsts.Id + " =? ";
                using (var cursor = ContentResolver.Query(Android.Provider.MediaStore.Images.Media.ExternalContentUri, null, selection, new string[] { doc_id }, null))
                {
                    if (cursor == null) return path;
                    var columnIndex = cursor.GetColumnIndexOrThrow(Android.Provider.MediaStore.Images.Media.InterfaceConsts.Data);
                    cursor.MoveToFirst();
                    path = cursor.GetString(columnIndex);
                }

            }
            catch (Exception ex)
            {

                //
            }

            return path;
        }
    }
}