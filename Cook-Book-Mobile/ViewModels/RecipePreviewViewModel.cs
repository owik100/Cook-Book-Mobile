using AutoMapper;
using Cook_Book_Mobile.Helpers;
using Cook_Book_Mobile.Models;
using Cook_Book_Mobile.Views;
using Cook_Book_Shared_Code.API;
using Cook_Book_Shared_Code.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace Cook_Book_Mobile.ViewModels
{
    public class RecipePreviewViewModel : BaseViewModel
    {
        private RecipeModel currentRecipe;

        private string _recipeName;
        private List<string> _recipeIntegradts;
        private string _recipeInstructions;
        private string _imagePath;
        private int _recipeId;
        private bool _canEdit;
        private string _userName;

        private string _favouritesImage;
        private bool _canAddDeleteFavourites;
        private AddOrdDeleteFromFavourites _AddOrdDeleteFavourites;

        private IRecipesEndPointAPI _recipesEndPointAPI;
        private IMapper _mapper;
        private ILoggedUser _loggedUser;
        private IAPIHelper _aPIHelper;

        private UserOrPublicOrFavouritesRecipes lastVised = UserOrPublicOrFavouritesRecipes.UserRecipes;

        public ICommand EditCommand { get; set; }
        public ICommand DeleteCommand { get; set; }
        public ICommand FavouriteCommand { get; set; }

        public RecipePreviewViewModel(IRecipesEndPointAPI recipesEndPointAPI, IMapper mapper, ILoggedUser loggedUser, IAPIHelper aPIHelper)
        {
            EditCommand = new Command(() => Edit());
            DeleteCommand = new Command(async () => await Delete());
            FavouriteCommand = new Command(async () => await AddOrDeleteFavourites());

            _recipesEndPointAPI = recipesEndPointAPI;
            _mapper = mapper;
            _loggedUser = loggedUser;
            _aPIHelper = aPIHelper;

            CanEdit = false;

            MessagingCenter.Subscribe<RecipesPage, RecipeAndTitlePage>(this, EventMessages.RecipesPreviewEvent, async (sender, arg) =>
            {

                if (arg.Title == "Moje przepisy")
                {
                    lastVised = UserOrPublicOrFavouritesRecipes.UserRecipes;
                }
                else if (arg.Title == "Odkrywaj przepisy")
                {
                    lastVised = UserOrPublicOrFavouritesRecipes.PublicResipes;
                }
                else if (arg.Title == "Ulubione przepisy")
                {
                    lastVised = UserOrPublicOrFavouritesRecipes.FavouritesRecipes;
                }

                RecipeModel recipeModel = _mapper.Map<RecipeModel>(arg.RecipeModelDisplay);

                currentRecipe = recipeModel;
                Title = arg.RecipeModelDisplay.Name;

                _recipeId = currentRecipe.RecipeId;
                RecipeName = currentRecipe.Name;
                RecipeIngredients = (currentRecipe.Ingredients).ToList();
                RecipeInstructions = currentRecipe.Instruction;
                ImagePath = currentRecipe.ImagePath;

                if (!currentRecipe.IsPublic || currentRecipe.UserName == _loggedUser.UserName)
                {
                    CanEdit = true;
                    CanAddDeleteFavourites = false;
                }
                else
                {
                    CanEdit = false;
                    UserName = "Autor przepisu: " + currentRecipe.UserName;

                    CanAddDeleteFavourites = true;

                    if (await AlreadyFavourites())
                    {
                        _AddOrdDeleteFavourites = AddOrdDeleteFromFavourites.Delete;
                        FavouritesImage = ImageConstants.StarFull;
                    }
                    else
                    {
                        _AddOrdDeleteFavourites = AddOrdDeleteFromFavourites.Add;
                        FavouritesImage = ImageConstants.StarEmpty;
                    }
                }
            });
        }

        private async Task Delete()
        {
            bool answer;

            try
            {
                answer = await Application.Current.MainPage.DisplayAlert(RecipeName, "Na pewno chcesz usunąć ten przepis? Operacji nie można cofnąć!", "Tak", "Nie");
                if (answer)
                {
                    var result = await _recipesEndPointAPI.DeleteRecipe(currentRecipe.RecipeId.ToString());

                    MessagingCenter.Send(this, EventMessages.BasicNavigationEvent);
                }
            }
            catch (Exception ex)
            {
                //_logger.Error("Got exception", ex);
                await Application.Current.MainPage.DisplayAlert("Błąd", ex.Message, "Ok");
            }
        }

        private void Edit()
        {
            MessagingCenter.Send(this, EventMessages.EditRecipeEvent, currentRecipe);
        }

        #region Props
        public string ImagePath
        {
            get { return _imagePath; }
            set
            {
                _imagePath = value;
                OnPropertyChanged(nameof(ImagePath));
            }
        }

        public string RecipeName
        {
            get { return _recipeName; }
            set
            {
                _recipeName = value;
                OnPropertyChanged(nameof(RecipeName));
            }
        }

        public List<string> RecipeIngredients
        {
            get { return _recipeIntegradts; }
            set
            {
                _recipeIntegradts = value;
                OnPropertyChanged(nameof(RecipeIngredients));
            }
        }

        public string RecipeInstructions
        {
            get { return _recipeInstructions; }
            set
            {
                _recipeInstructions = value;
                OnPropertyChanged(nameof(RecipeInstructions));
            }
        }
        public string UserName
        {
            get { return _userName; }
            set
            {
                _userName = value;
                OnPropertyChanged(nameof(UserName));
            }
        }

        public bool CanEdit
        {
            get { return _canEdit; }
            set
            {
                _canEdit = value;
                OnPropertyChanged(nameof(CanEdit));
            }
        }

        public string FavouritesImage
        {
            get { return _favouritesImage; }
            set
            {
                _favouritesImage = value;
                OnPropertyChanged(nameof(FavouritesImage));
            }
        }


        public bool CanAddDeleteFavourites
        {
            get { return _canAddDeleteFavourites; }
            set
            {
                _canAddDeleteFavourites = value;
                OnPropertyChanged(nameof(CanAddDeleteFavourites));
            }
        }
        #endregion

        public async Task AddOrDeleteFavourites()
        {
            try
            {
                if (_AddOrdDeleteFavourites == AddOrdDeleteFromFavourites.Add)
                {
                    _loggedUser.FavouriteRecipes.Add(_recipeId.ToString());
                }
                else
                {
                    _loggedUser.FavouriteRecipes.Remove(_recipeId.ToString());
                }

                LoggedUser loggedUser = new LoggedUser
                {
                    Id = _loggedUser.Id,
                    Email = _loggedUser.Email,
                    UserName = _loggedUser.UserName,
                    FavouriteRecipes = _loggedUser.FavouriteRecipes
                };

                var result = await _aPIHelper.EditUser(loggedUser);

                if (result)
                {
                    if (await AlreadyFavourites())
                    {
                        _AddOrdDeleteFavourites = AddOrdDeleteFromFavourites.Delete;
                        FavouritesImage = ImageConstants.StarFull;
                    }
                    else
                    {
                        _AddOrdDeleteFavourites = AddOrdDeleteFromFavourites.Add;
                        FavouritesImage = ImageConstants.StarEmpty;
                    }

                    if (lastVised == UserOrPublicOrFavouritesRecipes.PublicResipes)
                    {
                        MessagingCenter.Send(this, EventMessages.ReloadPublicRecipesEvent);
                    }
                    else if (lastVised == UserOrPublicOrFavouritesRecipes.FavouritesRecipes)
                    {
                        MessagingCenter.Send(this, EventMessages.ReloadFavouritesRecipesEvent);
                    }

                }

                //_reloadNeeded = true;

                //await Back();
            }
            catch (Exception ex)
            {
                // _logger.Error("Got exception", ex);
                await Application.Current.MainPage.DisplayAlert("Błąd", ex.Message, "Ok");
            }
        }

        private async Task<bool> AlreadyFavourites()
        {
            bool output = false;

            try
            {
                if (_loggedUser.FavouriteRecipes.Contains(_recipeId.ToString()))
                {
                    output = true;
                }
            }
            catch (Exception ex)
            {
                //_logger.Error("Got exception", ex);
                await Application.Current.MainPage.DisplayAlert("Błąd", ex.Message, "Ok");
            }

            return output;
        }
    }

    public enum AddOrdDeleteFromFavourites
    {
        Add,
        Delete
    }
}
