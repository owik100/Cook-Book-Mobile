using Cook_Book_Mobile.Helpers;
using Cook_Book_Mobile.Models;
using Cook_Book_Mobile.Views;
using Cook_Book_Shared_Code.API;
using Cook_Book_Shared_Code.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace Cook_Book_Mobile.ViewModels
{
    public class RecipesViewModel : BaseViewModel
    {
        private IRecipesEndPointAPI _recipesEndPointAPI;
        private ObservableCollection<RecipeModel> _recipes;
        private bool _isRefreshing;

        List<RecipeModel> tempRecipes = new List<RecipeModel>();

        public ICommand RefreshCommand { get; set; }

        public RecipesViewModel(IRecipesEndPointAPI RecipesEndPointAPI)
        {
            Title = "Twoje przepisy";

            RefreshCommand = new Command(async () => await RefreshData());

            _recipesEndPointAPI = RecipesEndPointAPI;

            MessagingCenter.Subscribe<MenuViewModel>(this, EventMessages.ReloadRecipesEvent, async (sender) =>
            {
                await RefreshData();
            });

            MessagingCenter.Subscribe<AddOrEditPage>(this, EventMessages.ReloadRecipesEvent, async (sender) =>
            {
                await RefreshData();
            });

            MessagingCenter.Subscribe<RecipePreviewPage>(this, EventMessages.ReloadRecipesEvent, async (sender) =>
            {
                await RefreshData();
            });

        }

        #region Props
        public ObservableCollection<RecipeModel> Recipes
        {
            get { return _recipes; }
            set
            {
                _recipes = value;
                OnPropertyChanged(nameof(Recipes));
            }
        }

        public bool IsRefreshing
        {
            get { return _isRefreshing; }
            set
            {
                _isRefreshing = value;
                OnPropertyChanged(nameof(IsRefreshing));
            }
        }
        #endregion

        private async Task RefreshData()
        {

           // IsRefreshing = true;
            await LoadRecipes();
             await LoadImages();

          //  await Task.WhenAll(LoadRecipes(), LoadImages());

            IsRefreshing = false;
        }

        private async Task LoadRecipes()
        {
            try
            {
                tempRecipes = await _recipesEndPointAPI.GetAllRecipesLoggedUser();
              
            }
            catch (Exception ex)
            {
                //_logger.Error("Got exception", ex);
                await Application.Current.MainPage.DisplayAlert("Błąd", ex.Message, "Ok");
            }
        }

        private async Task LoadImages()
        {
            try
            {
                List<string> DontDeletetheseImages = new List<string>();

                foreach (var item in tempRecipes)
                {
                    if (item.NameOfImage == null)
                    {
                        item.ImagePath = "Cook_Book_Mobile.Images.foodtemplate.png";
                        continue;
                    }

                    if (TempData.ImageExistOnDisk(item.NameOfImage))
                    {
                        item.ImagePath = TempData.GetImagePath(item.NameOfImage);
                        DontDeletetheseImages.Add(item.NameOfImage);
                        continue;
                    }

                    var downloadStatus = await _recipesEndPointAPI.DownloadImage(item.NameOfImage);

                    if (downloadStatus)
                    {

                        item.ImagePath = TempData.GetImagePath(item.NameOfImage);
                        DontDeletetheseImages.Add(item.NameOfImage);
                    }

                }

                // TempData.DeleteUnusedImages(DontDeletetheseImages);

                Recipes = new ObservableCollection<RecipeModel>(tempRecipes);
                OnPropertyChanged(nameof(Recipes));
            }
            catch (Exception ex)
            {
                //_logger.Error("Got exception", ex);
                await Application.Current.MainPage.DisplayAlert("Błąd", ex.Message, "Ok");
            }

        }

    }
}
