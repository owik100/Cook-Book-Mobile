using System;
using System.Reflection;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace Cook_Book_Mobile.ViewModels
{
    public class AboutViewModel : BaseViewModel
    {
        private string _appVersion;
        public AboutViewModel()
        {
            Title = "O aplikacji";
            AppVersion = Assembly.GetExecutingAssembly().GetName().Version.ToString();
            OpenWebCommand = new Command(async () => await Browser.OpenAsync("https://github.com/owik100/Cook-Book-Mobile"));
        }

        #region Props

        public string AppVersion
        {
            get { return _appVersion; }
            set
            {
                _appVersion = value;
                OnPropertyChanged(nameof(AppVersion));
            }
        }
        #endregion

        public ICommand OpenWebCommand { get; }
    }
}