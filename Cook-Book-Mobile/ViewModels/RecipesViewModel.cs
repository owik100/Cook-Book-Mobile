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

        List<RecipeModel> tempRecipes = new List<RecipeModel>();

        public RecipesViewModel(IRecipesEndPointAPI RecipesEndPointAPI)
        {
            Title = "Twoje przepisy";

            _recipesEndPointAPI = RecipesEndPointAPI;

            MessagingCenter.Subscribe<MenuViewModel>(this, EventMessages.ReloadRecipesEvent, async (sender) =>
            {
                await LoadRecipes();
                await LoadImages();
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
        #endregion

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
