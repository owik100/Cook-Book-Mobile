using AutoMapper;
using Cook_Book_Mobile.Helpers;
using Cook_Book_Mobile.Models;
using Cook_Book_Mobile.Views;
using Cook_Book_Shared_Code.API;
using Cook_Book_Shared_Code.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
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
        private IMapper _mapper;
        private ILoggedUser _loggedUser;
        private ObservableCollection<RecipeModelDisplay> _recipes;
        private bool _isRefreshing;
        private UserOrPublicRecipes _currentRecipes = UserOrPublicRecipes.UserRecipes;

        private bool _canNext;
        private bool _canPrevious;

        private int pageSize = 10;
        private int totalPages = 1;
        private int pageNumberUserRecipes = 1;
        private int pageNumberPublicRecipes = 1;

        private string _pageInfo;

        List<RecipeModelDisplay> tempRecipes = new List<RecipeModelDisplay>();

        public ICommand RefreshCommand { get; set; }
        public ICommand BackCommand { get; set; }
        public ICommand NextCommand { get; set; }

        public RecipesViewModel(IRecipesEndPointAPI RecipesEndPointAPI, ILoggedUser loggedUser, IMapper mapper)
        {
            Title = "Moje przepisy";

            RefreshCommand = new Command(async () => await RefreshData(_currentRecipes));
            BackCommand = new Command(async () => await RecipesBack());
            NextCommand = new Command(async () => await RecipesNext());

            _recipesEndPointAPI = RecipesEndPointAPI;
            _loggedUser = loggedUser;
            _mapper = mapper;

            MessagingCenter.Subscribe<MenuViewModel>(this, EventMessages.ReloadUserRecipesEvent, async (sender) =>
            {
                _currentRecipes = UserOrPublicRecipes.UserRecipes;
                await RefreshData(_currentRecipes);
            });

            MessagingCenter.Subscribe<MenuViewModel>(this, EventMessages.ReloadPublicRecipesEvent, async (sender) =>
            {
                _currentRecipes = UserOrPublicRecipes.PublicResipes;
                await RefreshData(_currentRecipes);
            });

            MessagingCenter.Subscribe<AddOrEditPage>(this, EventMessages.ReloadUserRecipesEvent, async (sender) =>
            {
                _currentRecipes = UserOrPublicRecipes.UserRecipes;
                await RefreshData(_currentRecipes);
            });

            MessagingCenter.Subscribe<RecipePreviewPage>(this, EventMessages.ReloadUserRecipesEvent, async (sender) =>
            {
                _currentRecipes = UserOrPublicRecipes.UserRecipes;
                await RefreshData(_currentRecipes);
            });

            MessagingCenter.Subscribe<MenuViewModel>(this, EventMessages.LogOffEvent, (sender) =>
            {
                LogOffUser();
            });

        }

        #region Props
        public ObservableCollection<RecipeModelDisplay> Recipes
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

        public bool CanNext
        {
            get { return _canNext; }
            set
            {
                _canNext = value;
                OnPropertyChanged(nameof(CanNext));
            }
        }
        public bool CanPrevious
        {
            get { return _canPrevious; }
            set
            {
                _canPrevious = value;
                OnPropertyChanged(nameof(CanPrevious));
            }
        }
        public string PageInfo
        {
            get { return _pageInfo; }
            set
            {
                _pageInfo = $"Strona {value} z {totalPages}";
                OnPropertyChanged(nameof(PageInfo));
            }
        }
        #endregion

        private async Task RefreshData(UserOrPublicRecipes RecipesToRefresh)
        {
            // IsRefreshing = true;
            try
            {
                if (!IsBusy)
                {
                    IsBusy = true;
                    if (RecipesToRefresh == UserOrPublicRecipes.UserRecipes)
                    {
                        await LoadUserRecipes(pageSize, pageNumberUserRecipes);
                    }
                    else if (RecipesToRefresh == UserOrPublicRecipes.PublicResipes)
                    {
                        await LoadPublicRecipes(pageSize, pageNumberPublicRecipes);
                    }

                    //  await Task.WhenAll(LoadRecipes(), LoadImages());
                    await LoadImages();
                    IsRefreshing = false;
                }
            }
            catch (Exception ex)
            {
                //throw;
            }
            finally
            {
                IsBusy = false;
            }        
        }

        private async Task LoadUserRecipes(int pageSize, int pageNumber)
        {
            try
            {
                Title = "Moje przepisy";
                tempRecipes.Clear();
                var recipes = await _recipesEndPointAPI.GetRecipesLoggedUser(pageSize, pageNumberUserRecipes);

                totalPages = recipes.FirstOrDefault().TotalPages;
                PageInfo = pageNumber.ToString();

                NavigationButtonsActiveDeactive(pageNumber);

                RecipeModelsToRecipeModelDisplay(recipes);
                await LoadImages();
            }
            catch (Exception ex)
            {
                //_logger.Error("Got exception", ex);
                await Application.Current.MainPage.DisplayAlert("Błąd", ex.Message, "Ok");
            }
        }

        private async Task LoadPublicRecipes(int pageSize, int pageNumber)
        {
            try
            {
                Title = "Odkrywaj przepisy";
                tempRecipes.Clear();
                var recipes = await _recipesEndPointAPI.GetPublicRecipes(pageSize, pageNumberPublicRecipes);

                totalPages = recipes.FirstOrDefault().TotalPages;
                PageInfo = pageNumber.ToString();

                NavigationButtonsActiveDeactive(pageNumber);

                RecipeModelsToRecipeModelDisplay(recipes);
                await LoadImages();
            }
            catch (Exception ex)
            {
                //_logger.Error("Got exception", ex);
                await Application.Current.MainPage.DisplayAlert("Błąd", ex.Message, "Ok");
            }
        }

        private async void RecipeModelsToRecipeModelDisplay(List<RecipeModel> recipeModels)
        {
            try
            {
                foreach (var item in recipeModels)
                {
                    RecipeModelDisplay recipeModelDisplaySingle = _mapper.Map<RecipeModelDisplay>(item);

                    bool displayAsPublic = false;
                    if (item.IsPublic && _loggedUser.UserName == item.UserName)
                    {
                        displayAsPublic = true;
                    }
                    recipeModelDisplaySingle.DisplayAsPublic = displayAsPublic;

                    tempRecipes.Add(recipeModelDisplaySingle);
                }
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
                        item.ImagePath = ImageConstants.LoadDefaultImage;
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
                 TempData.DeleteUnusedImages(DontDeletetheseImages);

                Recipes = new ObservableCollection<RecipeModelDisplay>(tempRecipes);
                OnPropertyChanged(nameof(Recipes));
            }
            catch (Exception ex)
            {
                //_logger.Error("Got exception", ex);
                await Application.Current.MainPage.DisplayAlert("Błąd", ex.Message, "Ok");
            }

        }

        public async Task RecipesBack()
        {
            if (_currentRecipes == UserOrPublicRecipes.UserRecipes)
            {
                await LoadUserRecipes(pageSize, --pageNumberUserRecipes);
            }
            else
            {
                await LoadPublicRecipes(pageSize, --pageNumberPublicRecipes);
            }
        }

        public async Task RecipesNext()
        {
            if (_currentRecipes == UserOrPublicRecipes.UserRecipes)
            {
                await LoadUserRecipes(pageSize, ++pageNumberUserRecipes);
            }
            else
            {
                await LoadPublicRecipes(pageSize, ++pageNumberPublicRecipes);
            }

        }

        private void NavigationButtonsActiveDeactive(int pageNumber)
        {
            if (pageNumber <= 1)
            {
                CanPrevious = false;
            }
            else
            {
                CanPrevious = true;
            }

            if (pageNumber >= totalPages)
            {
                CanNext = false;
            }
            else
            {
                CanNext = true;
            }
        }

        public void LogOffUser()
        {
            _currentRecipes = UserOrPublicRecipes.UserRecipes;
            CanNext = false;
            CanPrevious = false;

            pageSize = 10;
            totalPages = 1;
            pageNumberUserRecipes = 1;
            pageNumberPublicRecipes = 1;

            tempRecipes.Clear();
            _recipes.Clear();
        }

    }
}

public enum UserOrPublicRecipes
{
    UserRecipes,
    PublicResipes
}
