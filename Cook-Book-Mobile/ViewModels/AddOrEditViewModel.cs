using Cook_Book_Shared_Code.API;
using Cook_Book_Shared_Code.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace Cook_Book_Mobile.ViewModels
{
    public class AddOrEditViewModel : BaseViewModel
    {
        public ICommand SubmitCommand { get; set; }
        private string _recipeName;
        private ObservableCollection<string> _recipeIntegradts = new ObservableCollection<string>();
        private string _selectedIngredient;
        private string _ingredientInsert;
        private string _recipeInstructions;
        private string _image;

        private IRecipesEndPointAPI _recipesEndPointAPI;

        private AddOrEdit _addOrEdit = AddOrEdit.Add;
        private string _submitText;
        private int _recipeId;

        private bool reloadNeeded = false;

        public AddOrEditViewModel(IRecipesEndPointAPI RecipesEndPointAPI)
        {
            _recipesEndPointAPI = RecipesEndPointAPI;

            SubmitCommand = new Command (async () => await Submit());

            Title = "Dodaj";
            ImagePath = "Cook_Book_Mobile.Images.foodtemplate.png";
        }

        #region Props
        public string SubmitText
        {
            get { return _submitText; }
            set
            {
                _submitText = value;
                OnPropertyChanged(nameof(SubmitText));
            }
        }

        public string ImagePath
        {
            get { return _image; }
            set
            {
                _image = value;
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
                OnPropertyChanged(nameof(CanRecipeSubmit));
            }
        }

        public ObservableCollection<string> RecipeIngredients
        {
            get { return _recipeIntegradts; }
            set
            {
                _recipeIntegradts = value;
                OnPropertyChanged(nameof(RecipeIngredients));
                OnPropertyChanged(nameof(CanRecipeSubmit));
            }
        }

        public string SelectedIngredient
        {
            get { return _selectedIngredient; }
            set
            {
                _selectedIngredient = value;
                OnPropertyChanged(nameof(SelectedIngredient));
                OnPropertyChanged(nameof(RecipeIngredients));
                OnPropertyChanged(nameof(CanDeleteIngredient));
            }
        }

        public string RecipeInstructions
        {
            get { return _recipeInstructions; }
            set
            {
                _recipeInstructions = value;
                OnPropertyChanged(nameof(RecipeInstructions));
                OnPropertyChanged(nameof(CanRecipeSubmit));
            }
        }

        public string IngredientInsert
        {
            get { return _ingredientInsert; }
            set
            {
                _ingredientInsert = value;
                OnPropertyChanged(nameof(IngredientInsert));
                OnPropertyChanged(nameof(CanAddIngredientTextBox));
            }
        }

        public bool CanAddIngredientTextBox
        {
            get
            {
                bool output = false;

                if (!string.IsNullOrWhiteSpace(IngredientInsert))
                {
                    output = true;
                }

                return output;
            }

        }

        public bool CanDeleteIngredient
        {
            get
            {
                bool output = false;

                if (!string.IsNullOrWhiteSpace(SelectedIngredient))
                {
                    output = true;
                }

                return output;
            }

        }

        public bool CanDeleteFileModel
        {
            get
            {
                bool output = false;

                if (!string.IsNullOrWhiteSpace(ImagePath) && ImagePath != "Cook_Book_Mobile.Images.foodtemplate.png")
                {
                    output = true;
                }

                return output;
            }
        }

        public bool CanRecipeSubmit
        {
            get
            {
                bool output = false;

                if (!string.IsNullOrWhiteSpace(RecipeName) && !string.IsNullOrWhiteSpace(RecipeInstructions) && RecipeIngredients.Count >= 0)
                {
                    output = true;
                }

                return output;
            }
        }

        #endregion

        public async Task Submit()
        {
            try
            {
                RecipeModel recipeModel = new RecipeModel
                {
                    Name = RecipeName,
                    Ingredients = RecipeIngredients.ToList(),
                    Instruction = RecipeInstructions,
                    NameOfImage = ImagePath
                };

                if (recipeModel.NameOfImage == "Cook_Book_Mobile.Images.foodtemplate.png")
                {
                    recipeModel.NameOfImage = "";
                }

                if (_addOrEdit == AddOrEdit.Add)
                {
                    await _recipesEndPointAPI.InsertRecipe(recipeModel);
                    reloadNeeded = true;

                    //await _eventAggregator.PublishOnUIThreadAsync(new LogOnEvent(reloadNeeded), new CancellationToken());
                }
                else if (_addOrEdit == AddOrEdit.Edit)
                {
                    recipeModel.RecipeId = _recipeId;
                    var result = await _recipesEndPointAPI.EditRecipe(recipeModel);

                    if (result)
                    {
                        if (recipeModel.NameOfImage == "")
                        {
                            recipeModel.NameOfImage = "Cook_Book_Mobile.Images.foodtemplate.png";
                        }

                        OnPropertyChanged(nameof(ImagePath));
                        OnPropertyChanged(nameof(CanDeleteFileModel));

                        reloadNeeded = true;
                        await Application.Current.MainPage.DisplayAlert("Zaktualizowano pomyślnie!", "Zaktualizowano", "Ok");
                    }
                }
            }
            catch (Exception ex)
            {
              // _logger.Error("Got exception", ex);
                await Application.Current.MainPage.DisplayAlert("Błąd", ex.Message, "Ok");
            }
        }

    }

    public enum AddOrEdit
    {
        Add,
        Edit
    }
}
