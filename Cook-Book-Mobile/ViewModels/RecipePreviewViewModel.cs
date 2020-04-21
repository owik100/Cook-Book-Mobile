using AutoMapper;
using Cook_Book_Mobile.API;
using Cook_Book_Mobile.Helpers;
using Cook_Book_Mobile.Views;
using Cook_Book_Shared_Code.API;
using Cook_Book_Shared_Code.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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

        private IRecipesEndPointAPI _recipesEndPointAPI;
        private IMapper _mapper;
        private ILoggedUser _loggedUser;

        public ICommand EditCommand { get; set; }
        public ICommand DeleteCommand { get; set; }

        public RecipePreviewViewModel(IRecipesEndPointAPI recipesEndPointAPI, IMapper mapper, ILoggedUser loggedUser)
        {
            EditCommand = new Command(() => Edit());
            DeleteCommand = new Command(async () => await Delete());

            _recipesEndPointAPI = recipesEndPointAPI;
            _mapper = mapper;
            _loggedUser = loggedUser;

            CanEdit = false;

            MessagingCenter.Subscribe<RecipesPage, RecipeModelDisplay>(this, EventMessages.RecipesPreviewEvent, (sender, arg) =>
            {
                RecipeModel recipeModel = _mapper.Map<RecipeModel>(arg);

                currentRecipe = recipeModel;
                Title = arg.Name;

                _recipeId = currentRecipe.RecipeId;
                RecipeName = currentRecipe.Name;
                RecipeIngredients = (currentRecipe.Ingredients).ToList();
                RecipeInstructions = currentRecipe.Instruction;
                ImagePath = currentRecipe.ImagePath;

                if (!currentRecipe.IsPublic || currentRecipe.UserName == _loggedUser.UserName)
                {
                    CanEdit = true;
                }
                else
                {
                    CanEdit = false;
                    UserName = "Autor przepisu: " + currentRecipe.UserName;
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
           MessagingCenter.Send(this, EventMessages.EditRecipeEvent,currentRecipe);
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
        #endregion
    }
}
