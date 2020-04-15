using Cook_Book_Mobile.Helpers;
using Cook_Book_Mobile.Views;
using Cook_Book_Shared_Code.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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

        public ICommand EditCommand { get; set; }
        public ICommand DeleteCommand { get; set; }

        public RecipePreviewViewModel()
        {
            EditCommand = new Command(() => Edit());
            DeleteCommand = new Command(() => Delete());

            MessagingCenter.Subscribe<RecipesPage, RecipeModel>(this, EventMessages.RecipesPreviewEvent, (sender, arg) =>
            {
                currentRecipe = arg;
                Title = arg.Name;

                _recipeId = currentRecipe.RecipeId;
                RecipeName = currentRecipe.Name;
                RecipeIngredients = (currentRecipe.Ingredients).ToList();
                RecipeInstructions = currentRecipe.Instruction;
                ImagePath = currentRecipe.ImagePath;
            });
        }

        private void Delete()
        {
            throw new NotImplementedException();
        }

        private void Edit()
        {
            throw new NotImplementedException();
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
        #endregion
    }
}
